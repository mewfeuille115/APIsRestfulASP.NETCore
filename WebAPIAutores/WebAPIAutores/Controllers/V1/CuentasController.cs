using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAutores.DTOs;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers.V1
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CuentasController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly HashService _hashService;
		private readonly IDataProtector _dataProtector;

		public CuentasController(
			UserManager<IdentityUser> userManager,
			IConfiguration configuration,
			SignInManager<IdentityUser> signInManager,
			IDataProtectionProvider dataProtectionProvider,
			HashService hashService)
		{
			_userManager = userManager;
			_configuration = configuration;
			_signInManager = signInManager;
			_hashService = hashService;
			_dataProtector = dataProtectionProvider.CreateProtector("valor_unico_y_quizas_secreto");
		}

		[HttpGet("Hash/{textoPlano}")]
		public ActionResult RealizarHash(string textoPlano)
		{
			var resultado1 = _hashService.Hash(textoPlano);
			var resultado2 = _hashService.Hash(textoPlano);

			return Ok(new
			{
				textoPlano,
				Hash1 = resultado1,
				Hash2 = resultado2,
			});
		}

		[HttpGet("Encriptar")]
		public ActionResult Encriptar()
		{
			var textoPlano = "Sergio Pérez";
			var textoCifrado = _dataProtector.Protect(textoPlano);
			var textoDesencriptado = _dataProtector.Unprotect(textoCifrado);

			return Ok(new
			{
				textoPlano,
				textoCifrado,
				textoDesencriptado,
			});
		}

		[HttpGet("EncriptarPorTiempo")]
		public ActionResult EncriptarPorTiempo()
		{
			var protectorLimitadoPorTiempo = _dataProtector.ToTimeLimitedDataProtector();

			var textoPlano = "Sergio Pérez";
			var textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
			Thread.Sleep(6000);
			// La app va a tronar porque pasó el tiempo límite de desencriptación.
			var textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);

			return Ok(new
			{
				textoPlano,
				textoCifrado,
				textoDesencriptado,
			});
		}

		[HttpPost("Registrar", Name = "RegistarUsuario")]
		public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
		{
			var usuario = new IdentityUser
			{
				UserName = credencialesUsuario.Email,
				Email = credencialesUsuario.Email,
			};

			var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);

			if (!resultado.Succeeded)
				return BadRequest(resultado.Errors);

			return await ConstruirToken(credencialesUsuario);
		}

		[HttpPost("Login", Name = "LoginUsuario")]
		public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
		{
			var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuario.Email,
				credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

			if (!resultado.Succeeded)
				return BadRequest("Login incorrecto");

			return await ConstruirToken(credencialesUsuario);
		}

		[HttpGet("RenovarToken", Name = "RenovarToken")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
		{
			var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
			var email = emailClaim.Value;

			var credencialesUsuario = new CredencialesUsuario()
			{
				Email = email,
			};

			return await ConstruirToken(credencialesUsuario);
		}

		private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
		{
			var claims = new List<Claim>()
			{
				new Claim("email", credencialesUsuario.Email),
				new Claim("lo que yo quiera", "cualquier otro valor"),
			};

			var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
			var claimsDb = await _userManager.GetClaimsAsync(usuario);

			claims.AddRange(claimsDb);

			var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
			var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

			var expiracion = DateTime.UtcNow.AddDays(30);

			var securityToken = new JwtSecurityToken(
				issuer: null,
				audience: null,
				claims: claims,
				expires: expiracion,
				signingCredentials: creds
			);

			return new RespuestaAutenticacion()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
				Expiracion = expiracion,
			};
		}

		[HttpPost("HacerAdmin", Name = "HacerAdmin")]
		public async Task<ActionResult> HacerAdmin(EditarAdminDto editarAdminDto)
		{
			var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
			await _userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));

			return NoContent();
		}

		[HttpPost("RemoverAdmin", Name = "RemoverAdmin")]
		public async Task<ActionResult> RemoverAdmin(EditarAdminDto editarAdminDto)
		{
			var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
			await _userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));

			return NoContent();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RobsonDev.Services
{
    public static class PasswordCryptoHelper
    {
        #region Constantes
        private const int HashSize = 32;
        private const int IterationCount = 10000;
        private const int SaltSize = 32;
        #endregion

        #region Métodos/Operadores Privados
        /// <summary>
        /// Verificar se o conteúdo dos itens é igual.
        /// </summary>
        /// <param name="item1">Primeiro item.</param>
        /// <param name="item2">Segundo item.</param>
        /// <returns>Verdadeiro se iguais, caso contrário, falso.</returns>
        private static bool SlowEquals(IReadOnlyList<byte> item1, IReadOnlyList<byte> item2)
        {
            var diff = (uint)item1.Count ^ (uint)item2.Count;
            for (var i = 0; i < item1.Count && i < item2.Count; i++)
                diff |= (uint)(item1[i] ^ item2[i]);
            return diff == 0;
        }
        #endregion

        #region Métodos/Operadores Públicos
        /// <summary>
        /// Gerar o hash criptográfico da senha.
        /// </summary>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Resultado da operação.</returns>
        public static string GeneratePasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterationCount, HashAlgorithmName.SHA512);
            var hashData = rfc2898DeriveBytes.GetBytes(HashSize);
            var saltData = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(hashData) + ":" + Convert.ToBase64String(saltData);
        }

        /// <summary>
        /// Verificar a senha.
        /// </summary>
        /// <param name="password">Senha do usuário.</param>
        /// <param name="passwordHash">Hash da senha calculado anteriormente.</param>
        /// <returns>Verdadeiro se válida, caso contrário, falso.</returns>
        public static bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(password));

            var parts = passwordHash.Split(":");
            if (parts.Length != 2) throw new ArgumentOutOfRangeException(nameof(passwordHash));

            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterationCount, HashAlgorithmName.SHA512) { Salt = Convert.FromBase64String(parts[1]) };
            var hashData = rfc2898DeriveBytes.GetBytes(HashSize);
            return SlowEquals(hashData, Convert.FromBase64String(parts[0]));
        }
        #endregion
    }
}

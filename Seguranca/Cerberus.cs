using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SHA3;

namespace Framework.Utils.Seguranca
{
    public sealed class Cerberus
    {

        #region Constantes
        //As contantes abaixo podem ser modificadas sem preocupação de quebrar os hashes existentes
        // Quanto maior o Salt maior o tempo de busca pela vulnerabilidade
        //Porém, o custo para o processamento será maior. Pode ser mantido em 64 bits.
        //O tamanho do site influi diretamente no tamanho das iterações do PBKDF2
        private const Int16 TAMANHO_BYTE_SALT = 64;
        private const Int16 TAMANHO_BYTE_HASH = 64;

        //O tamanho do site influi diretamente no tamanho das iterações do PBKDF2
        private const Int16 ITERACOES_PBKDF2 = 10000;
        private const Int16 INDICE_ITERACAO = 0;
        private const Int16 INDICE_SALT = 1;
        private const Int16 INDICE_PBKDF2 = 2;

        #endregion

        #region Construtores

        //Singleton
        //Pelos métodos serem 
        //todos estáticos, então o construtor deve ser privado
        //Codigo Microsoft de design: CA1053
        private Cerberus()
        {

        }

        #endregion

        #region Métodos para Criptografia

        /// <summary>
        /// Cria um hash para senhas em MD5, este método também pode ser usado para 
        /// validar se a senha é ou não um hash MD5.
        /// </summary>
        /// <param name="senha">A senha para ser criado o hash</param>
        /// <returns>A senha convertida em Hash MD5</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public static string MD5(string senha)
        {
            if (String.IsNullOrEmpty(senha) || String.IsNullOrWhiteSpace(senha)){
                throw new ArgumentNullException("o parâmetro senha não pode estar em branco ou não pode ser nula!");
            }
            using (MD5 messageDigest5 = new MD5CryptoServiceProvider())
            {
                messageDigest5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(senha));

                byte[] hashSenha = messageDigest5.Hash;

                StringBuilder sb = new StringBuilder();
                sb.Append("0x");
                for (int i = 0; i < hashSenha.Length; i++)
                {
                    sb.Append(hashSenha[i].ToString("X2", CultureInfo.InvariantCulture));
                }

                return sb.ToString();
            }                       
        }

        /// <summary>
        /// Cria um hash salted em PBKDF2 hash da senha.
        /// </summary>
        /// <param name="password">A senha para o hash.</param>
        /// <returns>O hash da senha.</returns>
        public static String CriaHashParaSenhaComSaltUtilizandoPbkdf2(String senha)
        {
            //RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            if (String.IsNullOrEmpty(senha) || String.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentNullException("Senha não pode estar em branco ou não pode ser nula!");
            }

            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
            {
                Byte[] salt = new Byte[TAMANHO_BYTE_SALT];
                csprng.GetBytes(salt);

                // Gera o Hash da senha
                Byte[] hash = PBKDF2(senha, salt, ITERACOES_PBKDF2, TAMANHO_BYTE_HASH);
                return ITERACOES_PBKDF2 + ":" +
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash);
            }            
        }

        /// <summary>
        /// Valida uma senha dado o seu hash atual
        /// </summary>
        /// <param name="senha">A senha que precisa ser verificada.</param>
        /// <param name="hashSenha">O hash da senha.</param>
        /// <returns>Verdadeiro se a senha estiver correta. Falso se não for correta a senha.</returns>
        public static bool ValidaHashSenhaComSalt(string senha, string hashSenha)
        {
            if (String.IsNullOrEmpty(senha) || String.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentNullException("Senha não pode estar em branco ou não pode ser nula!");
            }

            if (String.IsNullOrEmpty(hashSenha) || String.IsNullOrWhiteSpace(hashSenha))
            {
                throw new ArgumentNullException("Hash não pode estar em branco ou não pode ser nula!");
            }
            // Extrai os valores do hash
            char[] delimiter = { ':' };
            string[] split = hashSenha.Split(delimiter);
            int iterations = Int32.Parse(split[INDICE_ITERACAO], CultureInfo.InvariantCulture);
            byte[] salt = Convert.FromBase64String(split[INDICE_SALT]);
            byte[] hash = Convert.FromBase64String(split[INDICE_PBKDF2]);

            byte[] testHash = PBKDF2(senha, salt, iterations, hash.Length);
            return ChecaByteAByte(hash, testHash);
        }

        /// <summary>
        /// Compara dois arrays de bytes em tempo de tamanho-constante. Quando este método de
        /// comparação é usado, evita um ataque de força bruta sobre o tempo do algoritmo
        /// Lembrando que quanto mais custoso para a quebra, mais dificil o ataque
        /// </summary>
        /// <param name="a">O primeiro array de byte</param>
        /// <param name="b">O segundo array de byte</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool ChecaByteAByte(byte[] a, byte[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("Argumento <a> não pode ser nulo");
            }

            if (b == null)
            {
                throw new ArgumentNullException("Argumento <b> não pode ser nulo");
            }

            uint diferenca = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diferenca |= (uint)(a[i] ^ b[i]);
            return diferenca == 0;
        }

        /// <summary>
        /// Cria um hash PBKDF2-SHA1 de uma senha
        /// </summary>
        /// <param name="senha">A senha para a geração do hash.</param>
        /// <param name="salt">O salt.</param>
        /// <param name="iteracoes">A contagem de iteracao do PBKDF2.</param>
        /// <param name="bytesSaida">O tamanho do hash a ser gerado, em bytes</param>
        /// <returns>O hash da senha.</returns>
        private static byte[] PBKDF2(string senha, byte[] salt, int iteracoes, int bytesSaida)
        {
            if (String.IsNullOrEmpty(senha) || String.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentNullException("É necessário que se passe uma senha!");
            } else if (salt == null){
                throw new ArgumentNullException("É necessário que se passe um array de bytes para o Salt!");
            } else if (iteracoes <= 0){
                throw new ArgumentNullException("É necessário que se o número de iterações!");
            } else if (bytesSaida <= 0){
                throw new ArgumentNullException("É necessário que se o número de bytes de saída!");
            }

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(senha, salt)){
                pbkdf2.IterationCount = iteracoes;
                return pbkdf2.GetBytes(bytesSaida);
            }                       
        }

        /// <summary>
        /// Gera um Hash para o valor em texto puro e retorna um resultado
        /// base64. Antes o hash é calculado, um salt aleatório é gerado e adicionado o texto puro. Este salt
        /// é armazenado no fim do valor do hash gerado, então pode ser usado depois para verificações.
        /// </summary>
        /// <param name="senha">
        /// A senha em texto puro que será converida em hash. Este método não verifica
        /// se o parâmetro é nulo.
        /// </param>
        /// <param name="algoritmoHash">
        /// Nome do algoritmo do hash. Até o momento, temos: "MD5", "SHA1",
        /// "SHA256", "SHA384", "SHA512" e "SHA3" (Se outro valor diferente destes
        /// o hashing do MD5 será utilizado). Estes valores são case insensitive.
        /// </param>
        /// <param name="salt">
        /// Bytes para o Salt hash. Este parâmetro pode ser nulo, neste caso um salt gerado aleatóriamnte
        /// será gerado
        /// </param>
        /// <returns>
        /// Valor de hash formatado como uma string base64.
        /// </returns>
        public static string GeraValorHash(String senha,
                                         String algoritmoHash,
                                         Byte[] salt)
        {
            // Se o salt não é informado, gera um na hora.
            if (salt == null)
            {
                //define o valor máximo e o mínino de salts
                // Define min and max salt sizes.
                int tamanhoMinSalt = 1;
                int tamanhoMaxSalt = TAMANHO_BYTE_SALT;

                //Gera um número aleatório para o tamaho do salt
                Random random = new Random();
                int tamanhoSalt = random.Next(tamanhoMinSalt, tamanhoMaxSalt);

                //Aloca o array de byte, que armzena o salta.
                salt = new byte[tamanhoSalt];

                //Inicia o gerado de números aleatórios.
                //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    //Preenche o hash do salt de forma criptográfica forte.
                    rng.GetNonZeroBytes(salt);
                }                                
            }

            //Converte a senha em texto puro dentro do array de bytes.
            byte[] TextoPuroBytes = Encoding.UTF8.GetBytes(senha);

            //Aloca o array, que armazena o texto puro e o salt.
            byte[] textoPuroComBytesSalt = new byte[TextoPuroBytes.Length + salt.Length];

            //Copia os bytes em texto puro dentro do array resultante.
            for (int i = 0; i < TextoPuroBytes.Length; i++)
                textoPuroComBytesSalt[i] = TextoPuroBytes[i];
            //anexa os bytes do hash salt para o array resultante.
            for (int i = 0; i < salt.Length; i++)
                textoPuroComBytesSalt[TextoPuroBytes.Length + i] = salt[i];

            //Como este método pode suportar multiplos algoritmos de hash, precisamos definir
            //os objetos de hashing com uma classe (abstrata) comum. 
            HashAlgorithm hash;

            //Faz com que o algoritmo de geração do hash seja informado
            if (algoritmoHash == null)
                algoritmoHash = String.Empty;

            //Inicializa a criação do hash de senha com o algoritmo selecionado
            switch (algoritmoHash.ToUpper(CultureInfo.CurrentCulture))
            {
                case "SHA1":
                    using (hash = new SHA1Managed())                    
                    break;

                case "SHA256":
                    using (hash = new SHA256Managed())                    
                    break;

                case "SHA384":                   
                    using (hash = new SHA384Managed())                    
                    break;

                case "SHA512":
                    using (hash = new SHA512Managed())                    
                    break;

                case "SHA3":
                    using (hash = new SHA3Managed(512))                    
                    break;

                default:
                    using (hash = new MD5CryptoServiceProvider())                                        
                    break;
            }

            //Calcula o valor do hash baseado em nossa senha em texto puro com o salt anexado.
            byte[] hashBytes = hash.ComputeHash(textoPuroComBytesSalt);

            //Cria um array que armazena o hash e os bytes Salts originais
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

            //Copia os bytes hash dentro do array resultante;
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            //Anexa bytes salta para o resultado
            for (int i = 0; i < salt.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = salt[i];

            //Converte o resultaado dentro de uma string base64.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            //Retorna o resultado.
            return hashValue;
        }

        /// <summary>
        /// Compara um hash de uma senha em texto puro de um hash dado.
        /// A senha em texto puro é convetida em hash com o mesmo valor salta do hash original
        /// </summary>
        /// <param name="senha">
        /// A senha em texto puro que será verificada com o hash informado. O método
        /// não valida se o parâmetro é nulo.
        /// </param>
        /// <param name="algoritmoHash">
        /// Nome do algoritmo do hash. Até o momento, temos: "MD5", "SHA1",
        /// "SHA256", "SHA384", e "SHA512" (Se outro valor diferente destes
        /// o hashing do MD5 será utilizado). Estes valores são case insensitive.
        /// </param>
        /// <param name="valorHash">
        /// Um valor de hash produzido pelo método GeraSenhaHash. Este valor inclui o valor de salta original
        /// anexado a ele.
        /// </param>
        /// <returns>
        /// Se o hash bater, o hash informado retornará true, caso contrário retornará false
        /// </returns>
        public static bool ValidaSenha(string senha,
                                      string algoritmoHash,
                                      string valorHash)
        {
            //Converte o valor de hash de base64 em um array de byte.
            byte[] hashComBytesSalt = Convert.FromBase64String(valorHash);

            //Precisamos saber o tamanho do hash (sem salt).
            int hashTamanhoEmBits, hashSizeEmBytes;

            //Forçe o nome do algoritmo de hash.
            if (algoritmoHash == null)
                algoritmoHash = String.Empty;

            //Tamanho do hash é baseado no algoritmo escolhido.
            //Palavras maiúsculas variam de País para País
            //Código microsoft: CA1304
            switch (algoritmoHash.ToUpper(CultureInfo.CurrentCulture))
            {
                case "SHA1":
                    hashTamanhoEmBits = 160;
                    break;

                case "SHA256":
                    hashTamanhoEmBits = 256;
                    break;

                case "SHA384":
                    hashTamanhoEmBits = 384;
                    break;

                case "SHA512":
                    hashTamanhoEmBits = 512;
                    break;

                case "SHA3":
                    hashTamanhoEmBits = 512;
                    break;

                default: // precisa ser MD5
                    hashTamanhoEmBits = 128;
                    break;
            }

            //Converte o tamanho do hash de bits para bytes.
            hashSizeEmBytes = hashTamanhoEmBits / 8;

            //Force o valor de hash especificado ser longo o suficiente.
            if (hashComBytesSalt.Length < hashSizeEmBytes)
                return false;

            //Aloque o array para armazenar os bytes originais do Salt.
            byte[] saltBytes = new byte[hashComBytesSalt.Length -
                                        hashSizeEmBytes];

            //Copie o salt do fim do hash para um novo array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashComBytesSalt[hashSizeEmBytes + i];

            //Calcule o novo string de hash.
            string expectedHashString =
                        GeraValorHash(senha, algoritmoHash, saltBytes);

            //Se o hash calculado é idêntico ao hash informado,
            //A senha em texto puro está correta.
            return (valorHash == expectedHashString);
        }

        #endregion

    }
}

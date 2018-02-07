namespace Eagle.Core.Encryption
{
    public enum EncryptionType { DES, EmailFileContent, FileContent }
    /// <summary>
    /// Interface for string and byte array encryption
    /// </summary>
    public interface IEncryption
    {
        byte[] Encrypt(byte[] input);
        string Encrypt(string text);

        byte[] Decrypt(byte[] input);
        string Decrypt(string text);
    }
}

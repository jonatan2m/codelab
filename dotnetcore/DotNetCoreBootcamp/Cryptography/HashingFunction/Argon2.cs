using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography.HashingFunction
{
    /// <summary>
    /// Hashing Function
    /// Using Konscious.Security.Cryptography library.
    /// 
    /// How to deal with passwords securetly
    /// 
    /// DO NOT STORE A PASSWORD IN CLEAR TEXT    
    /// If in 2020 you are saving passwords in clear text, you're really crazy or like to live dangerously.    
    ///
    /// DO NOT STORE AN ENCODED PASSWORD
    /// Enconding is better than clear text but Base64 wa not designed to secure data; it was designed to represent binary
    /// data in textual format. If you Base64 encode a passaword, anyone can Base64 decode it using an online tool or cmd.
    /// 
    /// DO NOT STORE AN ENCRYPTED PASSWORD
    /// Although encrypting data is better than storing enconded data, it is a two-way street.
    /// If someone has encrypted a secret, anyone with a key can decrypt it.
    /// And this is the fundamental problem of encryption – where do you store the key?
    /// The process that needs to decrypt the password must have access to the key.    
    /// 
    /// THE RIGHT WAY
    /// The best approach is to apply one-way cryptography.
    /// the one-way crypto turns very difficult to convert the hash into plain text again.
    /// The hacker community has taken advantage of this and pre-computed hashes for entire key spaces for certain algorithms.
    /// An attacker can use one of these rainbow tables to determine the clear text given an hash value.
    /// To solve this problem, we can use a salt. It is a random value and it is prepended or appended to the clear text
    /// before calculating the hash.
    /// Now, attackers can't use a rainbow table to know the clear text.
    /// The salt value could be stored on database, alongside of hash value.
    /// 
    /// Some hashing function algorithms:
    /// ARGON2ID, SCRYPT, BRCYPT, PBKDF2
    /// more info https://www.twelve21.io/how-to-securely-store-a-password/
    /// https://www.twelve21.io/how-to-use-argon2-for-password-hashing-in-csharp/
    /// </summary>
    public class Argon2
    {
        private byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; //four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; //1GB

            return argon2.GetBytes(16);
        }

        private bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }

        public static void Play(string password)
        {
            var argon2 = new Argon2();
            var stopwatch = Stopwatch.StartNew();
            
            var salt = argon2.CreateSalt();
            var hash = argon2.HashPassword(password, salt);

            stopwatch.Stop();

            System.Diagnostics.Trace.WriteLine($"Process took {stopwatch.ElapsedMilliseconds / 1024.0} s");

            stopwatch = Stopwatch.StartNew();

            var success = argon2.VerifyHash(password, salt, hash);

            System.Diagnostics.Trace.WriteLine(success ? "Success!" : "Failute!");

            stopwatch.Stop();
            System.Diagnostics.Trace.WriteLine($"Process took {stopwatch.ElapsedMilliseconds / 1024.0} s");
        }
    }
}

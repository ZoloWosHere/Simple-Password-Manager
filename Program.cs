using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

class PasswordManager
{
    private Dictionary<string, string> passwords;
    private string storageFile;

    public PasswordManager(string storageFile)
    {
        passwords = new Dictionary<string, string>();
        this.storageFile = storageFile;

        if (File.Exists(storageFile))
        {
            LoadPasswords();
        }
    }

    public void AddPassword(string website, string password)
    {
        passwords[website] = password;
        SavePasswords();
        Console.WriteLine($"Password added for {website}.");
    }

    public string GetPassword(string website)
    {
        if (passwords.ContainsKey(website))
        {
            return passwords[website];
        }
        else
        {
            Console.WriteLine($"No password found for {website}.");
            return null;
        }
    }

    private void LoadPasswords()
    {
        string[] lines = File.ReadAllLines(storageFile);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                string website = parts[0].Trim();
                string password = parts[1].Trim();
                passwords[website] = password;
            }
        }
    }

    private void SavePasswords()
    {
        List<string> lines = new List<string>();
        foreach (var pair in passwords)
        {
            string line = $"{pair.Key},{pair.Value}";
            lines.Add(line);
        }
        File.WriteAllLines(storageFile, lines);
        HideFile(storageFile);
    }

    private void HideFile(string file)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to hide the file: {ex.Message}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        string storageFile = "passwords.txt";
        PasswordManager passwordManager = new PasswordManager(storageFile);

        while (true)
        {
            Console.WriteLine("=== Password Manager ===");
            Console.WriteLine("1. Add Password");
            Console.WriteLine("2. Get Password");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter website: ");
                string website = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                passwordManager.AddPassword(website, password);
            }
            else if (choice == "2")
            {
                Console.Write("Enter website: ");
                string website = Console.ReadLine();

                string password = passwordManager.GetPassword(website);
                if (password != null)
                {
                    Console.WriteLine($"Password for {website}: {password}");
                }
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            Console.WriteLine();
        }
    }
}

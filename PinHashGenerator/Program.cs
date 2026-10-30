using System.Security;

Console.Write("Ingresa el PIN del SuperAdmin: ");
var pin = ReadSecret();

var hash = BCrypt.Net.BCrypt.HashPassword(pin);

Console.WriteLine();
Console.WriteLine("Hash BCrypt generado:");
Console.WriteLine(hash);

static string ReadSecret()
{
    var chars = new System.Collections.Generic.List<char>();

    while (true)
    {
        var key = Console.ReadKey(intercept: true);

        if (key.Key == ConsoleKey.Enter)
            break;

        if (key.Key == ConsoleKey.Backspace)
        {
            if (chars.Count > 0)
                chars.RemoveAt(chars.Count - 1);
            continue;
        }

        if (!char.IsControl(key.KeyChar))
            chars.Add(key.KeyChar);
    }

    return new string(chars.ToArray());
}

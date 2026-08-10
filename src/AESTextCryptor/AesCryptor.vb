Option Strict On

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

'Die AES-Verschlüsselung der Anwendung. Sie steckte früher als Private Sub in der Form und war
'damit nicht testbar. Alles hier drin ist Teil des Formats: ein Text, den eine ältere Version
'verschlüsselt hat, lässt sich nur entschlüsseln, wenn Kodierung, Iterationen, Hashverfahren und
'die Reihenfolge von Schlüssel und IV unverändert bleiben
Public NotInheritable Class AesCryptor
    'Kleinste erlaubte Länge des Saltwerts in Zeichen. Rfc2898DeriveBytes verlangt acht Bytes,
    'und UTF-32 macht aus jedem Zeichen vier Bytes, die Prüfung ist also strenger als nötig
    Public Const MinimumSaltLength As Integer = 8

    'Anzahl der Iterationen der Schlüsselableitung
    Private Const KeyDerivationIterations As Integer = 600000

    'Blockgröße von AES in Bit
    Private Const AesBlockSize As Integer = 128

    'Die Klasse hat nur geteilte Member und wird nie instanziert
    Private Sub New()
    End Sub

    'Verschlüsselt den Text und gibt ihn Base64-kodiert zurück
    Public Shared Function Encrypt(aesKeySize As Integer, plainText As String, password As String, salt As String) As String
        ValidateSalt(salt)

        Using aes As Aes = Aes.Create()
            InitializeAes(aes, aesKeySize, password, salt)

            Using encryptor = aes.CreateEncryptor()
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
                        Dim data = Encoding.UTF32.GetBytes(plainText) 'Daten verschlüsseln
                        cs.Write(data, 0, data.Length)
                        cs.FlushFinalBlock()
                    End Using

                    'ToArray liefert die Daten auch nach dem Schließen des Streams
                    Return Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
        End Using
    End Function

    'Entschlüsselt einen Base64-kodierten Text. Ein falsches Passwort führt zu einer
    'CryptographicException, eine ungültige Base64-Eingabe zu einer FormatException
    Public Shared Function Decrypt(aesKeySize As Integer, encryptedText As String, password As String, salt As String) As String
        ValidateSalt(salt)

        Dim data = Convert.FromBase64String(encryptedText)

        Using aes As Aes = Aes.Create()
            InitializeAes(aes, aesKeySize, password, salt)

            Using decryptor = aes.CreateDecryptor()
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, decryptor, CryptoStreamMode.Write)
                        cs.Write(data, 0, data.Length)
                        cs.FlushFinalBlock() 'Wirft bei falschem Passwort, weil der letzte Block nicht passt
                    End Using

                    Return Encoding.UTF32.GetString(ms.ToArray())
                End Using
            End Using
        End Using
    End Function

    'Prüft den Saltwert so, wie es die Oberfläche auch tut
    Private Shared Sub ValidateSalt(salt As String)
        If String.IsNullOrEmpty(salt) OrElse salt.Length < MinimumSaltLength Then
            Throw New ArgumentException($"The salt value must contain at least {MinimumSaltLength} characters.", NameOf(salt))
        End If
    End Sub

    'Setzt Schlüssellänge und Blockgröße und leitet Schlüssel und IV aus Passwort und Salt ab
    Private Shared Sub InitializeAes(aes As Aes, aesKeySize As Integer, password As String, salt As String)
        If Not aes.ValidKeySize(aesKeySize) Then
            Throw New ArgumentOutOfRangeException(NameOf(aesKeySize), aesKeySize, "The key size is not valid for AES.")
        End If

        aes.KeySize = aesKeySize 'möglich sind 128 oder 256 bit
        aes.BlockSize = AesBlockSize

        Using derivedKey As New Rfc2898DeriveBytes(password, Encoding.UTF32.GetBytes(salt), KeyDerivationIterations,
                                                   HashAlgorithmName.SHA256)
            'Die Reihenfolge gehört zum Format: GetBytes liefert einen Strom, der erste Aufruf den
            'Schlüssel, der zweite den IV
            aes.Key = derivedKey.GetBytes(aes.KeySize \ 8)
            aes.IV = derivedKey.GetBytes(aes.BlockSize \ 8)
        End Using
    End Sub
End Class

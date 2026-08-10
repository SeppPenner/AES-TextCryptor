Option Strict On

Imports System.Security.Cryptography

Imports Microsoft.VisualStudio.TestTools.UnitTesting

'Prüft die AES-Verschlüsselung der Anwendung
<TestClass>
Public Class AesCryptorTests

    'Ein mit AES-256 verschlüsselter Text muss unverändert wieder herauskommen
    <TestMethod>
    Public Sub EncryptAndDecryptWithAes256ReturnTheOriginalText()
        Dim encrypted = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                           TestDataProvider.Salt)
        Dim decrypted = AesCryptor.Decrypt(256, encrypted, TestDataProvider.Password, TestDataProvider.Salt)

        Assert.AreEqual(TestDataProvider.PlainText, decrypted)
    End Sub

    'Dasselbe mit AES-128, die zweite Auswahl in der ComboBox
    <TestMethod>
    Public Sub EncryptAndDecryptWithAes128ReturnTheOriginalText()
        Dim encrypted = AesCryptor.Encrypt(128, TestDataProvider.PlainText, TestDataProvider.Password,
                                           TestDataProvider.Salt)
        Dim decrypted = AesCryptor.Decrypt(128, encrypted, TestDataProvider.Password, TestDataProvider.Salt)

        Assert.AreEqual(TestDataProvider.PlainText, decrypted)
    End Sub

    'Der IV kommt aus der Schlüsselableitung und nicht aus einem Zufallsgenerator, dieselbe Eingabe
    'liefert also immer dieselbe Ausgabe. Das ist kryptografisch schwach, aber es ist das Format
    <TestMethod>
    Public Sub EncryptReturnsTheSameResultForTheSameInput()
        Dim first = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                       TestDataProvider.Salt)
        Dim second = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                        TestDataProvider.Salt)

        Assert.AreEqual(first, second)
    End Sub

    'AES-256 und AES-128 dürfen nicht dasselbe liefern, sonst wäre die Auswahl wirkungslos
    <TestMethod>
    Public Sub EncryptReturnsADifferentResultForTheTwoKeySizes()
        Dim aes256 = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                        TestDataProvider.Salt)
        Dim aes128 = AesCryptor.Encrypt(128, TestDataProvider.PlainText, TestDataProvider.Password,
                                        TestDataProvider.Salt)

        Assert.AreNotEqual(aes256, aes128)
    End Sub

    'Die Ausgabe ist Base64 und lässt sich zurückwandeln
    <TestMethod>
    Public Sub EncryptReturnsBase64()
        Dim encrypted = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                           TestDataProvider.Salt)

        'Ein 15 Zeichen langer Text sind in UTF-32 60 Bytes, aufgefüllt auf 64 Bytes
        Assert.AreEqual(64, Convert.FromBase64String(encrypted).Length)
    End Sub

    'Der wichtigste Test: der Referenzwert stammt aus Version 1.0.7.0. Wer Kodierung, Iterationen,
    'Hashverfahren oder die Reihenfolge von Schlüssel und IV ändert, macht alte Texte unlesbar
    <TestMethod>
    Public Sub EncryptProducesTheSameTextAsVersion107WithAes256()
        Dim encrypted = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password,
                                           TestDataProvider.Salt)

        Assert.AreEqual(TestDataProvider.EncryptedByVersion107Aes256, encrypted)
    End Sub

    'Dasselbe für AES-128
    <TestMethod>
    Public Sub EncryptProducesTheSameTextAsVersion107WithAes128()
        Dim encrypted = AesCryptor.Encrypt(128, TestDataProvider.PlainText, TestDataProvider.Password,
                                           TestDataProvider.Salt)

        Assert.AreEqual(TestDataProvider.EncryptedByVersion107Aes128, encrypted)
    End Sub

    'Ein von Version 1.0.7.0 verschlüsselter Text muss sich weiterhin entschlüsseln lassen
    <TestMethod>
    Public Sub DecryptReadsTheTextThatVersion107Encrypted()
        Dim decrypted = AesCryptor.Decrypt(256, TestDataProvider.EncryptedByVersion107Aes256,
                                           TestDataProvider.Password, TestDataProvider.Salt)

        Assert.AreEqual(TestDataProvider.PlainText, decrypted)
    End Sub

    'UTF-32 statt UTF-8 heißt: auch Zeichen außerhalb der Basic Multilingual Plane überleben den
    'Weg hin und zurück
    <TestMethod>
    Public Sub EncryptAndDecryptSurviveUmlautsAndSurrogatePairs()
        'Das Schloss-Emoji als Surrogatpaar, in UTF-16 zwei Char, in UTF-32 vier Bytes
        Dim text = "Grüße aus Hämmelsdorf, ößß, " & ChrW(&HD83D) & ChrW(&HDD10)

        Dim encrypted = AesCryptor.Encrypt(256, text, TestDataProvider.Password, TestDataProvider.Salt)
        Dim decrypted = AesCryptor.Decrypt(256, encrypted, TestDataProvider.Password, TestDataProvider.Salt)

        Assert.AreEqual(text, decrypted)
    End Sub

    'Die Oberfläche lässt keinen leeren Text zu, die Klasse muss ihn trotzdem verkraften
    <TestMethod>
    Public Sub EncryptAndDecryptHandleAnEmptyText()
        Dim encrypted = AesCryptor.Encrypt(256, String.Empty, TestDataProvider.Password, TestDataProvider.Salt)
        Dim decrypted = AesCryptor.Decrypt(256, encrypted, TestDataProvider.Password, TestDataProvider.Salt)

        Assert.AreNotEqual(String.Empty, encrypted) 'Ein aufgefüllter Block bleibt übrig
        Assert.AreEqual(String.Empty, decrypted)
    End Sub

    'Ein falsches Passwort scheitert am Auffüllen des letzten Blocks. Die Form fängt das ab und
    'schreibt "Ungültiges Passwort!" in die Ausgabebox
    <TestMethod>
    Public Sub DecryptWithAWrongPasswordThrowsACryptographicException()
        Assert.ThrowsExactly(Of CryptographicException)(
            Sub() AesCryptor.Decrypt(256, TestDataProvider.EncryptedByVersion107Aes256, "wrongPassword",
                                     TestDataProvider.Salt))
    End Sub

    'Ein falscher Saltwert wirkt genauso wie ein falsches Passwort
    <TestMethod>
    Public Sub DecryptWithAWrongSaltThrowsACryptographicException()
        Assert.ThrowsExactly(Of CryptographicException)(
            Sub() AesCryptor.Decrypt(256, TestDataProvider.EncryptedByVersion107Aes256,
                                     TestDataProvider.Password, "wrongSalt"))
    End Sub

    'Mit AES-128 kommt man an einen mit AES-256 verschlüsselten Text nicht heran
    <TestMethod>
    Public Sub DecryptWithTheWrongKeySizeThrowsACryptographicException()
        Assert.ThrowsExactly(Of CryptographicException)(
            Sub() AesCryptor.Decrypt(128, TestDataProvider.EncryptedByVersion107Aes256,
                                     TestDataProvider.Password, TestDataProvider.Salt))
    End Sub

    'Eine Eingabe, die kein Base64 ist, meldet sich als FormatException. Die Form zeigt dafür
    'denselben Text wie für ein falsches Passwort
    <TestMethod>
    Public Sub DecryptWithInvalidBase64ThrowsAFormatException()
        Assert.ThrowsExactly(Of FormatException)(
            Sub() AesCryptor.Decrypt(256, "this is not base64", TestDataProvider.Password, TestDataProvider.Salt))
    End Sub

    'Der Saltwert muss mindestens MinimumSaltLength Zeichen haben, sonst kommt die
    'Schlüsselableitung nicht mit den Bytes hin
    <TestMethod>
    Public Sub EncryptWithATooShortSaltThrowsAnArgumentException()
        Assert.ThrowsExactly(Of ArgumentException)(
            Sub() AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password, "short"))
    End Sub

    'Dieselbe Prüfung beim Entschlüsseln
    <TestMethod>
    Public Sub DecryptWithATooShortSaltThrowsAnArgumentException()
        Assert.ThrowsExactly(Of ArgumentException)(
            Sub() AesCryptor.Decrypt(256, TestDataProvider.EncryptedByVersion107Aes256,
                                     TestDataProvider.Password, String.Empty))
    End Sub

    'Eine Schlüssellänge, die AES nicht kennt, wird gemeldet und nicht stillschweigend korrigiert
    <TestMethod>
    Public Sub EncryptWithAnInvalidKeySizeThrowsAnArgumentOutOfRangeException()
        Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(
            Sub() AesCryptor.Encrypt(64, TestDataProvider.PlainText, TestDataProvider.Password,
                                     TestDataProvider.Salt))
    End Sub

    'Ein Saltwert von genau MinimumSaltLength Zeichen muss durchgehen, das ist die Grenze, gegen die
    'die Oberfläche prüft
    <TestMethod>
    Public Sub EncryptAcceptsASaltOfExactlyTheMinimumLength()
        Dim salt = New String("a"c, AesCryptor.MinimumSaltLength)

        Dim encrypted = AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password, salt)

        Assert.AreEqual(TestDataProvider.PlainText,
                        AesCryptor.Decrypt(256, encrypted, TestDataProvider.Password, salt))
    End Sub

    'Ein Zeichen unter der Grenze wird abgelehnt
    <TestMethod>
    Public Sub EncryptRejectsASaltOneCharacterBelowTheMinimumLength()
        Dim salt = New String("a"c, AesCryptor.MinimumSaltLength - 1)

        Assert.ThrowsExactly(Of ArgumentException)(
            Sub() AesCryptor.Encrypt(256, TestDataProvider.PlainText, TestDataProvider.Password, salt))
    End Sub
End Class

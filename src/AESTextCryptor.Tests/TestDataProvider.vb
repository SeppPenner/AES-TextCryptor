Option Strict On

'Die Werte, mit denen alle Tests arbeiten. Passwort, Salt und Text stecken auch in den beiden
'Referenzwerten aus Version 1.0.7.0, deshalb darf hier nichts geändert werden, ohne die
'Referenzwerte neu zu erzeugen
Friend NotInheritable Class TestDataProvider
    'Das Passwort für alle Tests
    Friend Const Password As String = "asdf1234"

    'Der Saltwert für alle Tests, genau MinimumSaltLength Zeichen lang
    Friend Const Salt As String = "asdfasdf"

    'Der Klartext für alle Tests, bewusst nur ASCII, damit die Referenzwerte nicht von der
    'Kodierung der Quelldatei abhängen
    Friend Const PlainText As String = "This is a test."

    'Der Text von oben, verschlüsselt mit AES-256 von Version 1.0.7.0
    Friend Const EncryptedByVersion107Aes256 As String =
        "vc8SO93CuJCEnK4zcJ2phr5aIyQPkzQ/tChMxEsf9eWw7jEip/GTmxWnZDxWXf6EQEOanuIQmNLjEwNY7zIX7g=="

    'Der Text von oben, verschlüsselt mit AES-128 von Version 1.0.7.0
    Friend Const EncryptedByVersion107Aes128 As String =
        "wvEa0n38E3nMu8Lzv645+nhm8Dz9KrT6fljcXhwWq6hTLWewkwkhCNvigOxCJ+57LwCQi2p/oslBEVp7QF3Dng=="

    'Nur geteilte Member, wird nie instanziert
    Private Sub New()
    End Sub
End Class

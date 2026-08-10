Option Strict On

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Public Class Main
    Dim _salt() As Byte 'Saltwert erzeugen
    Dim _sprache As String 'Sprache erzeugen
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Datei öffnen und Sprache auslesen:
        Try
            'Die Config.ini liegt neben der Exe. AppContext.BaseDirectory liefert das Verzeichnis auch
            'dann, wenn Assembly.Location leer ist, also bei einem Single-File-Publish
            Dim configFile = Path.Combine(AppContext.BaseDirectory, "Config.ini")

            'Nur lesend öffnen und nur, wenn die Datei da ist. Vorher wurde sie zum Lesen mit
            'Schreibzugriff geöffnet und bei Bedarf angelegt, was im Installationsverzeichnis unter
            'Programme ohne Administratorrechte fehlschlägt
            If File.Exists(configFile) Then
                For Each configLine In File.ReadLines(configFile)
                    If Not String.IsNullOrWhiteSpace(configLine) Then
                        Me._sprache = configLine.Trim() 'Sprache festsetzen, die letzte gefüllte Zeile gewinnt
                    End If
                Next
            End If
        Catch ex As Exception
            Call Me.ShowError(ex) 'Fehlermeldung ausgeben
        End Try
        'Verschlüsselungsarten anzeigen:
        Me.ComboBox_Art.Items.Add("AES-256") 'AES-256 als Verschlüsselungsart hinzufügen
        Me.ComboBox_Art.Items.Add("AES-128") 'AES-128 als Verschlüsselungsart hinzufügen
        Me.ComboBox_Art.SelectedIndex = 0 'Vorauswahl setzen, dass Combobox nicht leer
        'Sprache anpassen:
        Select Case Me._sprache
            Case "DE"
                Call Me.AllesAufDeutsch() 'Alles auf Deutsch übersetzen
                Me.RadioButton_Deutsch.Checked = True 'RadioButton_Deutsch auswählen
            Case Else
                Call Me.AllesAufEnglisch() 'Alles auf Englisch übersetzen
                Me.RadioButton_Englisch.Checked = True 'RadioButton_Englisch auswählen
        End Select
    End Sub

    Private Sub RadioButton_Deutsch_CheckedChanged(sender As Object, e As EventArgs) _
        Handles RadioButton_Deutsch.CheckedChanged
        If Me.RadioButton_Deutsch.Checked = True Then
            Call Me.AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        Else
            Call Me.AllesAufEnglisch() 'Alles auf Englisch übersetzen
        End If
    End Sub

    Private Sub RadioButton_Englisch_CheckedChanged(sender As Object, e As EventArgs) _
        Handles RadioButton_Englisch.CheckedChanged
        If Me.RadioButton_Englisch.Checked = True Then
            Call Me.AllesAufEnglisch() 'Alles auf Englisch übersetzen
        Else
            Call Me.AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        End If
    End Sub

    Private Sub AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        Me.Text = "AES Textverschlüsselung" 'Text von Form setzen
        Me.Label_Art.Text = "Bitte Verschlüsselungsart auswählen:" 'Label_Art Text setzen
        Me.Label_Salt.Text = "Salteingabe:" 'Label_Salt Text setzen
        Me.Label_Passwort.Text = "Passworteingabe:" 'Label_Passwort Text setzen
        Me.Label_Eingabe.Text = "Texteingabe:" 'Label_Eingabe Text setzen
        Me.Label_Ausgabe.Text = "Textausgabe:" 'Label_Ausgabe Text setzen
        Me.Button_Verschluesseln.Text = "Verschlüsseln" 'Button_Verschluesseln Text setzen
        Me.Button_Entschluesseln.Text = "Entschlüsseln" 'Button_Entschluesseln Text setzen
        Me.Button_Alle_Resetten.Text = "Alles löschen" 'Button_Alle_Resetten Text setzen
        Me.Label_Sprache.Text = "Sprache auswählen:" 'Label_Sprache Text setzen
        Me.RadioButton_Deutsch.Text = "Deutsch" 'RadioButton_Deutsch Text setzen
        Me.RadioButton_Englisch.Text = "Englisch" 'RadioButton_Englisch Text setzen
        Me._sprache = "DE" 'Sprache festlegen
    End Sub

    Private Sub AllesAufEnglisch() 'Alles auf Englisch übersetzen
        Me.Text = "AES Text Cryptor" 'Text von Form setzen
        Me.Label_Art.Text = "Choose encryption method:" 'Label_Art Text setzen
        Me.Label_Salt.Text = "Salt input:" 'Label_Salt Text setzen
        Me.Label_Passwort.Text = "Password input:" 'Label_Passwort Text setzen
        Me.Label_Eingabe.Text = "Text input:" 'Label_Eingabe Text setzen
        Me.Label_Ausgabe.Text = "Text output:" 'Label_Ausgabe Text setzen
        Me.Button_Verschluesseln.Text = "Encrypt" 'Button_Verschluesseln Text setzen
        Me.Button_Entschluesseln.Text = "Decrypt" 'Button_Entschluesseln Text setzen
        Me.Button_Alle_Resetten.Text = "Clear all" 'Button_Alle_Resetten Text setzen
        Me.Label_Sprache.Text = "Choose language:" 'Label_Sprache Text setzen
        Me.RadioButton_Deutsch.Text = "German" 'RadioButton_Deutsch Text setzen
        Me.RadioButton_Englisch.Text = "English" 'RadioButton_Englisch Text setzen
        Me._sprache = "EN" 'Sprache festlegen
    End Sub

    Private Sub Button_Verschluesseln_Click(sender As Object, e As EventArgs) Handles Button_Verschluesseln.Click _
        'Text verschlüsseln
        Try
            Select Case Me.ComboBox_Art.SelectedIndex
                Case 0 'AES-256 ausgewählt
                    If Me.RichTextBox_Passwort.Text = "" Or Me.RichTextBox_Eingabe.Text = "" Then 'Wenn Felder leer sind
                        Call Me.ShowWarning("Passwort oder Texteingabe ist leer",
                                            "Password or text is empty") 'Hinweis ausgeben
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Call Me.ShowWarning("Saltwert muss mindestens 8 Zeichen enthalten",
                                                "Salt value must contain at least 8 characters") 'Hinweis ausgeben
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.EncryptAes(256, Me.RichTextBox_Eingabe.Text, Me.RichTextBox_Passwort.Text) _
                            'Verschlüsselung aufrufen
                            Me.RichTextBox_Ausgabe.Clear() 'Ausgabebox leeren
                            Me.RichTextBox_Ausgabe.Text = Me._encryptedString 'Verschlüsselten Text ausgeben
                        End If
                    End If
                Case 1 'AES-128 ausgewählt
                    If Me.RichTextBox_Passwort.Text = "" Or Me.RichTextBox_Eingabe.Text = "" Then 'Wenn Felder leer sind
                        Call Me.ShowWarning("Passwort oder Texteingabe ist leer",
                                            "Password or text is empty") 'Hinweis ausgeben
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Call Me.ShowWarning("Saltwert muss mindestens 8 Zeichen enthalten",
                                                "Salt value must contain at least 8 characters") 'Hinweis ausgeben
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.EncryptAes(128, Me.RichTextBox_Eingabe.Text, Me.RichTextBox_Passwort.Text) _
                            'Verschlüsselung aufrufen
                            Me.RichTextBox_Ausgabe.Clear() 'Ausgabebox leeren
                            Me.RichTextBox_Ausgabe.Text = Me._encryptedString 'Verschlüsselten Text ausgeben
                        End If
                    End If
            End Select
        Catch ex As Exception
            Call Me.ShowError(ex) 'Fehlermeldung ausgeben
        End Try
    End Sub

    Private Sub Button_Entschluesseln_Click(sender As Object, e As EventArgs) Handles Button_Entschluesseln.Click
        Try
            Select Case Me.ComboBox_Art.SelectedIndex
                Case 0 'AES-256 ausgewählt
                    If Me.RichTextBox_Passwort.Text = "" Or Me.RichTextBox_Eingabe.Text = "" Then 'Wenn Felder leer sind
                        Call Me.ShowWarning("Passwort oder Texteingabe ist leer",
                                            "Password or text is empty") 'Hinweis ausgeben
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Call Me.ShowWarning("Saltwert muss mindestens 8 Zeichen enthalten",
                                                "Salt value must contain at least 8 characters") 'Hinweis ausgeben
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.DecryptAes(256, Me.RichTextBox_Eingabe.Text, Me.RichTextBox_Passwort.Text) _
                            'Entschlüsselung aufrufen
                            Me.RichTextBox_Ausgabe.Clear() 'Ausgabebox leeren
                            Me.RichTextBox_Ausgabe.Text = Me._decryptedString 'Entschlüsselten Text ausgeben
                        End If
                    End If
                Case 1 'AES-128 ausgewählt
                    If Me.RichTextBox_Passwort.Text = "" Or Me.RichTextBox_Eingabe.Text = "" Then 'Wenn Felder leer sind
                        Call Me.ShowWarning("Passwort oder Texteingabe ist leer",
                                            "Password or text is empty") 'Hinweis ausgeben
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Call Me.ShowWarning("Saltwert muss mindestens 8 Zeichen enthalten",
                                                "Salt value must contain at least 8 characters") 'Hinweis ausgeben
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.DecryptAes(128, Me.RichTextBox_Eingabe.Text, Me.RichTextBox_Passwort.Text) _
                            'Entschlüsselung aufrufen
                            Me.RichTextBox_Ausgabe.Clear() 'Ausgabebox leeren
                            Me.RichTextBox_Ausgabe.Text = Me._decryptedString 'Entschlüsselten Text ausgeben
                        End If
                    End If
            End Select
        Catch ex As Exception
            Call Me.ShowError(ex) 'Fehlermeldung ausgeben
        End Try
    End Sub

    Private Sub Button_Alle_Resetten_Click(sender As Object, e As EventArgs) Handles Button_Alle_Resetten.Click
        Me.RichTextBox_Salt.Clear() 'RichTextBox_Salt leeren
        Me.RichTextBox_Passwort.Clear() 'RichTextBox_Passwort leeren
        Me.RichTextBox_Eingabe.Clear() 'RichTextBox_Eingabe leeren
        Me.RichTextBox_Ausgabe.Clear() 'RichTextBox_Ausgabe leeren
    End Sub

    Private Sub ShowError(ex As Exception) 'Fehlermeldung mit Titel und Symbol ausgeben
        Dim titel As String
        Select Case Me._sprache
            Case "DE"
                titel = "Fehler" 'Titel auf Deutsch
            Case Else
                titel = "Error" 'Titel auf Englisch
        End Select

        MessageBox.Show(ex.Message, titel, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub ShowWarning(deutscherText As String, englischerText As String) 'Hinweis mit Titel und Symbol ausgeben
        Dim titel As String
        Dim text As String
        Select Case Me._sprache
            Case "DE"
                titel = "Hinweis" 'Titel auf Deutsch
                text = deutscherText
            Case Else
                titel = "Notice" 'Titel auf Englisch
                text = englischerText
        End Select

        MessageBox.Show(text, titel, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private _encryptedString As String
    Private _decryptedString As String

    ' Verschlüsseln
    Private Sub EncryptAes(aesKeySize As Integer, decryptedString As String, password As String)

        Dim generierterKey As New Rfc2898DeriveBytes(password, Me._salt, 600000, HashAlgorithmName.SHA256)
        Dim aes As Aes = Aes.Create()
        aes.KeySize = aesKeySize ' möglich sind 128 oder 256 bit
        aes.BlockSize = 128

        ' Algorithmus initialisieren:
        aes.Key = generierterKey.GetBytes(aes.KeySize \ 8)
        aes.IV = generierterKey.GetBytes(aes.BlockSize \ 8)

        ' Memory-Stream und Crypto-Stream erzeugen -> CreateEncryptor()
        Dim ms As New MemoryStream
        Dim cs As New CryptoStream(ms, aes.CreateEncryptor(),
                                   CryptoStreamMode.Write)

        ' Daten verschlüsseln:
        Dim data() As Byte
        data = Encoding.UTF32.GetBytes(decryptedString)
        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()
        cs.Close()

        ' Verschlüsselte Daten als String ausgeben: 
        Me._encryptedString = Convert.ToBase64String(ms.ToArray)
        ms.Close()

        aes.Clear()
    End Sub

    ' Entschlüsseln
    Private Sub DecryptAes(aesKeySize As Int32, encryptedString As String, password As String)

        Dim generierterKey As New Rfc2898DeriveBytes(password, Me._salt, 600000, HashAlgorithmName.SHA256)
        ' Instanzierung des AES-Algorithmus-Objekts:
        Dim aes As Aes = Aes.Create()
        ' Ein mit 256 bit verschlüsselter String kann 
        ' auch nur mit 256 bit entschlüsselt werden!
        aes.KeySize = aesKeySize ' möglich sind 128 oder 256 bit
        aes.BlockSize = 128

        ' Algorithmus initialisieren:
        aes.Key = generierterKey.GetBytes(aes.KeySize \ 8)
        aes.IV = generierterKey.GetBytes(aes.BlockSize \ 8)

        ' Memory-Stream und Crypto-Stream erzeugen -> CreateDecryptor()
        Dim ms As New MemoryStream
        Dim cs As New CryptoStream(ms, aes.CreateDecryptor(),
                                   CryptoStreamMode.Write)

        Try ' Daten entschlüsseln:
            Dim data() As Byte
            data = Convert.FromBase64String(encryptedString)
            cs.Write(data, 0, data.Length)
            cs.FlushFinalBlock()
            cs.Close()

            ' Die entschlüsselten Daten als String ausgeben: 
            Me._decryptedString = Encoding.UTF32.GetString(ms.ToArray)
            ms.Close()

            aes.Clear()
        Catch ex As Exception
            Select Case Me._sprache
                Case "DE"
                    Me._decryptedString = "Ungültiges Passwort!"
                Case Else
                    Me._decryptedString = "Wrong password!"
            End Select

        End Try
    End Sub
End Class

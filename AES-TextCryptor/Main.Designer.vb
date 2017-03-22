Imports System.ComponentModel
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()>
Partial Class Main
    Inherits Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(Main))
        Me.ComboBox_Art = New ComboBox()
        Me.Label_Art = New Label()
        Me.Label_Eingabe = New Label()
        Me.RichTextBox_Eingabe = New RichTextBox()
        Me.RichTextBox_Ausgabe = New RichTextBox()
        Me.Label_Ausgabe = New Label()
        Me.Button_Verschluesseln = New Button()
        Me.Button_Entschluesseln = New Button()
        Me.Label_Salt = New Label()
        Me.RichTextBox_Salt = New RichTextBox()
        Me.Label_Passwort = New Label()
        Me.RichTextBox_Passwort = New RichTextBox()
        Me.Button_Alle_Resetten = New Button()
        Me.RadioButton_Deutsch = New RadioButton()
        Me.RadioButton_Englisch = New RadioButton()
        Me.Label_Sprache = New Label()
        Me.SuspendLayout()
        '
        'ComboBox_Art
        '
        Me.ComboBox_Art.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ComboBox_Art.FormattingEnabled = True
        Me.ComboBox_Art.Location = New Point(12, 25)
        Me.ComboBox_Art.Name = "ComboBox_Art"
        Me.ComboBox_Art.Size = New Size(182, 21)
        Me.ComboBox_Art.TabIndex = 0
        '
        'Label_Art
        '
        Me.Label_Art.AutoSize = True
        Me.Label_Art.Location = New Point(12, 9)
        Me.Label_Art.Name = "Label_Art"
        Me.Label_Art.Size = New Size(0, 13)
        Me.Label_Art.TabIndex = 1
        '
        'Label_Eingabe
        '
        Me.Label_Eingabe.AutoSize = True
        Me.Label_Eingabe.Location = New Point(12, 248)
        Me.Label_Eingabe.Name = "Label_Eingabe"
        Me.Label_Eingabe.Size = New Size(0, 13)
        Me.Label_Eingabe.TabIndex = 2
        '
        'RichTextBox_Eingabe
        '
        Me.RichTextBox_Eingabe.Location = New Point(15, 264)
        Me.RichTextBox_Eingabe.Name = "RichTextBox_Eingabe"
        Me.RichTextBox_Eingabe.Size = New Size(707, 96)
        Me.RichTextBox_Eingabe.TabIndex = 3
        Me.RichTextBox_Eingabe.Text = ""
        '
        'RichTextBox_Ausgabe
        '
        Me.RichTextBox_Ausgabe.Location = New Point(15, 396)
        Me.RichTextBox_Ausgabe.Name = "RichTextBox_Ausgabe"
        Me.RichTextBox_Ausgabe.Size = New Size(707, 96)
        Me.RichTextBox_Ausgabe.TabIndex = 4
        Me.RichTextBox_Ausgabe.Text = ""
        '
        'Label_Ausgabe
        '
        Me.Label_Ausgabe.AutoSize = True
        Me.Label_Ausgabe.Location = New Point(12, 379)
        Me.Label_Ausgabe.Name = "Label_Ausgabe"
        Me.Label_Ausgabe.Size = New Size(0, 13)
        Me.Label_Ausgabe.TabIndex = 5
        '
        'Button_Verschluesseln
        '
        Me.Button_Verschluesseln.Location = New Point(200, 25)
        Me.Button_Verschluesseln.Name = "Button_Verschluesseln"
        Me.Button_Verschluesseln.Size = New Size(87, 23)
        Me.Button_Verschluesseln.TabIndex = 6
        Me.Button_Verschluesseln.UseVisualStyleBackColor = True
        '
        'Button_Entschluesseln
        '
        Me.Button_Entschluesseln.Location = New Point(293, 25)
        Me.Button_Entschluesseln.Name = "Button_Entschluesseln"
        Me.Button_Entschluesseln.Size = New Size(87, 23)
        Me.Button_Entschluesseln.TabIndex = 7
        Me.Button_Entschluesseln.UseVisualStyleBackColor = True
        '
        'Label_Salt
        '
        Me.Label_Salt.AutoSize = True
        Me.Label_Salt.Location = New Point(15, 64)
        Me.Label_Salt.Name = "Label_Salt"
        Me.Label_Salt.Size = New Size(0, 13)
        Me.Label_Salt.TabIndex = 8
        '
        'RichTextBox_Salt
        '
        Me.RichTextBox_Salt.Location = New Point(15, 82)
        Me.RichTextBox_Salt.Name = "RichTextBox_Salt"
        Me.RichTextBox_Salt.Size = New Size(707, 58)
        Me.RichTextBox_Salt.TabIndex = 9
        Me.RichTextBox_Salt.Text = ""
        '
        'Label_Passwort
        '
        Me.Label_Passwort.AutoSize = True
        Me.Label_Passwort.Location = New Point(15, 155)
        Me.Label_Passwort.Name = "Label_Passwort"
        Me.Label_Passwort.Size = New Size(0, 13)
        Me.Label_Passwort.TabIndex = 10
        '
        'RichTextBox_Passwort
        '
        Me.RichTextBox_Passwort.Location = New Point(14, 171)
        Me.RichTextBox_Passwort.Name = "RichTextBox_Passwort"
        Me.RichTextBox_Passwort.Size = New Size(707, 58)
        Me.RichTextBox_Passwort.TabIndex = 11
        Me.RichTextBox_Passwort.Text = ""
        '
        'Button_Alle_Resetten
        '
        Me.Button_Alle_Resetten.Location = New Point(386, 25)
        Me.Button_Alle_Resetten.Name = "Button_Alle_Resetten"
        Me.Button_Alle_Resetten.Size = New Size(87, 23)
        Me.Button_Alle_Resetten.TabIndex = 13
        Me.Button_Alle_Resetten.UseVisualStyleBackColor = True
        '
        'RadioButton_Deutsch
        '
        Me.RadioButton_Deutsch.AutoSize = True
        Me.RadioButton_Deutsch.Location = New Point(747, 50)
        Me.RadioButton_Deutsch.Name = "RadioButton_Deutsch"
        Me.RadioButton_Deutsch.Size = New Size(14, 13)
        Me.RadioButton_Deutsch.TabIndex = 14
        Me.RadioButton_Deutsch.TabStop = True
        Me.RadioButton_Deutsch.UseVisualStyleBackColor = True
        '
        'RadioButton_Englisch
        '
        Me.RadioButton_Englisch.AutoSize = True
        Me.RadioButton_Englisch.Location = New Point(819, 50)
        Me.RadioButton_Englisch.Name = "RadioButton_Englisch"
        Me.RadioButton_Englisch.Size = New Size(14, 13)
        Me.RadioButton_Englisch.TabIndex = 15
        Me.RadioButton_Englisch.TabStop = True
        Me.RadioButton_Englisch.UseVisualStyleBackColor = True
        '
        'Label_Sprache
        '
        Me.Label_Sprache.AutoSize = True
        Me.Label_Sprache.Location = New Point(744, 30)
        Me.Label_Sprache.Name = "Label_Sprache"
        Me.Label_Sprache.Size = New Size(0, 13)
        Me.Label_Sprache.TabIndex = 16
        '
        'Main
        '
        Me.AutoScaleDimensions = New SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(957, 504)
        Me.Controls.Add(Me.Label_Sprache)
        Me.Controls.Add(Me.RadioButton_Englisch)
        Me.Controls.Add(Me.RadioButton_Deutsch)
        Me.Controls.Add(Me.Button_Alle_Resetten)
        Me.Controls.Add(Me.RichTextBox_Passwort)
        Me.Controls.Add(Me.Label_Passwort)
        Me.Controls.Add(Me.RichTextBox_Salt)
        Me.Controls.Add(Me.Label_Salt)
        Me.Controls.Add(Me.Button_Entschluesseln)
        Me.Controls.Add(Me.Button_Verschluesseln)
        Me.Controls.Add(Me.Label_Ausgabe)
        Me.Controls.Add(Me.RichTextBox_Ausgabe)
        Me.Controls.Add(Me.RichTextBox_Eingabe)
        Me.Controls.Add(Me.Label_Eingabe)
        Me.Controls.Add(Me.Label_Art)
        Me.Controls.Add(Me.ComboBox_Art)
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.Name = "Main"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_Art As ComboBox
    Friend WithEvents Label_Art As Label
    Friend WithEvents Label_Eingabe As Label
    Friend WithEvents RichTextBox_Eingabe As RichTextBox
    Friend WithEvents RichTextBox_Ausgabe As RichTextBox
    Friend WithEvents Label_Ausgabe As Label
    Friend WithEvents Button_Verschluesseln As Button
    Friend WithEvents Button_Entschluesseln As Button
    Friend WithEvents Label_Salt As Label
    Friend WithEvents RichTextBox_Salt As RichTextBox
    Friend WithEvents Label_Passwort As Label
    Friend WithEvents RichTextBox_Passwort As RichTextBox
    Friend WithEvents Button_Alle_Resetten As Button
    Friend WithEvents RadioButton_Deutsch As RadioButton
    Friend WithEvents RadioButton_Englisch As RadioButton
    Friend WithEvents Label_Sprache As Label

End Class

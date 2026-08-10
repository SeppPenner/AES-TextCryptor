# Project rules for Claude

## What this is

AES-TextCryptor is a small **VB.NET WinForms** application that encrypts and decrypts text in a
single window. The user picks AES-256 or AES-128, types a salt, a password and the text, and gets
the result as a Base64 string in the output box. There is no file handling, no command line and no
library: the repository builds exactly one executable plus an Inno Setup installer. It is **not**
published as a NuGet package.

One solution `src/AESTextCryptor.sln` with exactly two projects:

- `src/AESTextCryptor/AESTextCryptor.vbproj`, `OutputType` `WinExe`, `UseWindowsForms`,
  `StartupObject` `AESTextCryptor.Main`. A `Form` is the startup object, so there is no `Program.vb`
  and no `My Project` folder.
- `src/AESTextCryptor.Tests/AESTextCryptor.Tests.vbproj`, MSTest, added in version 1.0.8.0. It
  targets the same `net*-windows` flavour as the application, because it references a Windows Forms
  executable.

Layout inside `src/AESTextCryptor`:

- `Main.vb`: the form. Language handling in `Form1_Load`, the two translation routines
  `AllesAufDeutsch` and `AllesAufEnglisch`, the three button handlers and the small private helpers
  around them (`EingabenSindGueltig`, `GewaehlteSchluessellaenge`, `ShowError`, `ShowWarning`).
- `AesCryptor.vb`: the crypto, `Encrypt` and `Decrypt` as `Shared` functions plus the private
  `ValidateSalt` and `InitializeAes`. No Windows Forms reference, which is what makes it testable.
  It knows nothing about languages: it throws, the form turns that into a message.
- `Main.Designer.vb` plus `Main.resx`: designer generated, `Main.resx` holds nothing but the form
  icon. Do not hand edit the designer file beyond what the designer itself would write.
- `Config.ini`: one line, `EN` or `DE`, copied to the output directory with
  `CopyToOutputDirectory=Always`.
- `AES.ico`: the application icon, `ApplicationIcon` and `SetupIconFile`,
  `CopyToOutputDirectory=Never`.
- `License.txt`: copied to the output directory, identical to the `License.txt` in the repository
  root, and the `LicenseFile` of the installer.

Layout inside `src/AESTextCryptor.Tests`:

- `AesCryptorTests.vb`: the roundtrip for both key sizes, the fact that the same input always gives
  the same output, Base64 and length, the two reference values from version 1.0.7.0, umlauts and a
  surrogate pair, the empty text, a wrong password, a wrong salt, the wrong key size, invalid Base64,
  the salt length boundary and an invalid key size.
- `TestDataProvider.vb`: password, salt, plain text and the two Base64 strings that version 1.0.7.0
  produced from them. **Do not change these values.** They are the only guard against a change that
  silently makes every text encrypted by an older version unreadable. The plain text is ASCII on
  purpose, so that the reference values do not depend on the encoding of the source file.

Repository root: `README.md` (the only user documentation), `Changelog.md`, `License.txt` (MIT),
`Screenshot_DE.PNG`, `Screenshot_EN.PNG`, `.gitattributes`, `.gitignore` and the `Setup` folder.
There is no `Updating.md` and no `HowToUse.md`.

`Setup` contains the Inno Setup script `AES-TextCryptor-Skript.iss` (German file name, kept),
the publish helper `build-setup-files.bat` and the built installer `AES-TextCryptor-Setup.exe`.
The installer is tracked although `.gitignore` excludes `*.exe`, so it needs `git add -f`.

## Build

```powershell
dotnet build src/AESTextCryptor.sln -c Release
```

```powershell
dotnet test src/AESTextCryptor.sln -c Release
```

- Single target framework `net9.0-windows` in both projects, no multi-targeting,
  `RuntimeIdentifiers` `win-x64`.
- All build properties live directly in the two `.vbproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository.
- A clean build reports zero warnings, keep it that way. `NuGetAudit` and `NuGetAuditMode=all` are
  on, so a vulnerable transitive package breaks the build.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.8-1` for the first
  commit after tag `1.0.7`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/AESTextCryptor.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest in the single test project `src/AESTextCryptor.Tests`, `dotnet test` runs 19 of
  them. They need no network and no fixture outside the repository, they touch neither the file
  system nor the form. Never claim a test run happened without running it.
- Beyond the tests, a behaviour change in the form is verified by starting the executable and doing a
  roundtrip by hand: encrypt a text, copy the output back into the input box, decrypt it and compare.
- The installer is built by `Setup/build-setup-files.bat` followed by `ISCC.exe` on
  `Setup/AES-TextCryptor-Skript.iss`. The batch file deletes every `bin` and `obj` below `src`,
  publishes into `src/AESTextCryptor/bin/publish` and removes the `*.pdb` files. It ends with
  `pause`, so it wants a console.

## Code conventions

Follow the surrounding code, it is consistent throughout `Main.vb`:

- `Option Strict On` at the top of every file, then the `Imports`. Every file declares its own
  `Imports`, the project declares no `<Import>` items, so nothing relies on implicit imports.
- **Comments are German**, with real umlauts, and they sit at the end of the line they explain
  (`Me.RichTextBox_Salt.Clear() 'RichTextBox_Salt leeren`). There are no XML doc comments anywhere
  in this repository, do not start adding them to single files only.
- Identifiers mix languages and that is on purpose: the control names and the German
  `Button_Verschluesseln` style come from the designer, method names such as `EncryptAes` are
  English. New members follow whatever the neighbouring member does.
- Fields, properties and methods are always accessed with `Me.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning` in `src/.editorconfig`).
- `Nullable` and `LangVersion latest` are enabled. `ImplicitUsings` is set as well, which does
  nothing for VB, it comes from the project template.
- `src/.editorconfig` enforces four spaces, CRLF and UTF-8. Most of its other rules are C# specific
  (`csharp_*`, file scoped namespaces) and do not apply here.
- Analyzer warnings are fixed, not silenced.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The language is read at startup and never written back.** `Form1_Load` reads `Config.ini`, the
  two radio buttons only switch the texts of the running instance. `README.md` documents exactly
  that: to change the language permanently you edit the file. The radio buttons are not broken.
- **The config reader takes the last filled line of the file, not the first.** The loop overwrites
  `_sprache` on every line that is not blank. Anything other than `DE` selects English, because the
  `Select Case` has no `Case "EN"` but a `Case Else`. A missing or empty `Config.ini` therefore
  yields English, and so does a file the application cannot read.
- **UTF-32 everywhere.** The salt, the plain text and the decrypted text all go through
  `Encoding.UTF32`. That is unusual, but it is part of the format on the wire: a text encrypted by
  an older version only decrypts because the encoding is still UTF-32. Never switch this to UTF-8.
- **The salt needs eight characters, not eight bytes.** The form and `AesCryptor` both check against
  `AesCryptor.MinimumSaltLength`, and `Rfc2898DeriveBytes` wants at least eight **bytes**. Since
  UTF-32 produces four bytes per character, eight characters are 32 bytes, so the check is stricter
  than the framework requires, not weaker.
- **Key and IV both come from the same `Rfc2898DeriveBytes` instance, in that order.** `GetBytes` is
  a stream: the first call returns the key, the second the IV. Swapping the two lines or deriving
  them separately breaks every text ever encrypted by this application. Same for the 600000
  iterations and `HashAlgorithmName.SHA256`.
- **`Aes.Create()` defaults are part of the format too**, that is CBC and PKCS7. They are never set
  explicitly, only `KeySize` and `BlockSize` are.
- **Encryption is deterministic.** The IV comes out of the key derivation instead of a random
  generator, so the same text, password and salt always produce the same Base64 string. That is
  weaker than a random IV, but it is the format, and a test pins it down.
- **Decryption reports "wrong password" for a wrong password and for invalid Base64.** The form
  catches `CryptographicException` and `FormatException` from `AesCryptor.Decrypt` and puts a
  localized text into the output box instead of a dialog. Every other exception is a real error and
  reaches the error dialog.
- **`src/Config.ini` and `src/AES.ico` are duplicates.** They sit one directory above the copies in
  `src/AESTextCryptor` and belong to the folder layout from before version 1.0.2.0. Nothing
  references them, they are tracked, leave them alone unless asked.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`.gitattributes` sets `* text=auto`**, every rule of the Visual Studio template below it is
  commented out. A binary file that must not be normalized needs its own rule.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-10)** : Short description.`
3. Set `MyAppVersion` in `Setup/AES-TextCryptor-Skript.iss` to the same four part version.
4. Commit that.
5. Tag the commit with the plain version number, no `v` prefix (`1.0.8`, `1.0.7`, ...). The existing
   tags are lightweight tags, create new ones the same way. **The tag comes before the installer
   build**, otherwise GitVersion burns a prerelease version into the shipped executable.
6. Run `Setup/build-setup-files.bat`, then `ISCC.exe Setup/AES-TextCryptor-Skript.iss`.
7. `git add -f Setup/AES-TextCryptor-Setup.exe` and commit it, the usual message is
   `Updated setup.`.
8. Push the commits and the tag.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.8-1+Branch.master.Sha...`. There is no package to push, so the release ends with the push.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed. That is what the history does, even though the code comments are German.
- Code comments stay **German** here, see the code conventions above. Comments in project files such
  as `.vbproj` and in the batch and Inno Setup files are English.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies, code comments) always use real umlauts and ß, never
  ASCII transliterations such as `ae`, `oe`, `ue` or `ss`.

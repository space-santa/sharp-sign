fsharpc sign.fs
mkbundle -o sign_pi --simple sign.exe --cross mono-5.10.0-raspbian-9-arm
mkbundle -o sign_mac --simple sign.exe --cross mono-5.10.0-osx-10.7-x64
mkbundle -o sign --simple sign.exe
mkbundle -o sign_x64 --simple sign.exe --cross mono-5.10.0-ubuntu-12.04-x64

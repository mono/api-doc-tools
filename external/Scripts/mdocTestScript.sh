cd ../../../..
cp external/Windows/Windows.Foundation.UniversalApiContract.winmd mdoc/mdoc.Test/bin/$1
cp external/Windows/Windows.WinMD mdoc/mdoc.Test/bin/$1 
cp external/Windows/Windows.Foundation.FoundationContract.winmd mdoc/mdoc.Test/bin/$1 
cp external/Test/UwpTestWinRtComponentCpp.dll mdoc/mdoc.Test/bin/$1 
cp external/Test/UwpTestWinRtComponentCpp.winmd mdoc/mdoc.Test/bin/$1 
cp external/Test/CSharpExample.dll mdoc/mdoc.Test/bin/$1 

sleep 5
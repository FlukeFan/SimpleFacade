@CD /D "%~dp0"
@title Simple Facade Command Prompt
@SET PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
@doskey b=msbuild $* SimpleFacade.proj
type readme.txt
%comspec%

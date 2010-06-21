@echo off
call build.bat
start lib\cassini.exe "%cd%\SampleApp" 8113
start http://localhost:8113/quartz/index.ashx
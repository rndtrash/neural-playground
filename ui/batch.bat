@echo off
FOR %%A IN (*.svg) DO inkscape %%A --export-width 24 --export-type=png
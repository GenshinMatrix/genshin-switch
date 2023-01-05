import os

for rt, dirs, files in os.walk('.'):
    for f in files:
        fname, ext = os.path.splitext(f)
        if ext == '.png' and '+' in fname:
            try:
                os.rename(f, f[0 : fname.find('+')] + ext)
            except:
                pass

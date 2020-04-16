# MarlinProbeAssistedLeveler
 A tool to physically level a 3D printer's bed using a BLTouch probe

# WORK IN PROGRESS SOFTWARE
# USE AT YOUR OWN RISK!!

## What does it do?
This tool is to assist in physically leveling your 3D printer's bed using a BLTouch probe
It probes all four corners of the bed, and works out the height difference between the four points.
Then, for each corner that is lower than the highest corner, it moves to that corner, deploys the probe, then moves the Z carriage down to a point where the BLTouch would trigger if the bed is at the perfect height.
You then wind the knob on that corner until the BLTouch triggers

## How do I use it?
Currently it is implemented as a console application, you need to build it yourself in Visual Studio and edit values in the source code.
**Before using, ensure that you have your Z offset for your probe configured!**
If / when I get it working well, I will look into making it a GUI application or even implementing it as an OctoPrint plugin
Currently it is Windows only


## Contributions
If you are interested in getting involved, I am more than willing to collaborate, drop me a line...

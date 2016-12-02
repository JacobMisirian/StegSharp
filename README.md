# StegSharp

Steganography Library and Console Application in C#

## Using the Console Application

### Encrypting a File Inside an Image

In order to encrypt a file inside an image, the image file and data file must
be passed on the command line, as well as an output file for the new image.

Use the --file flag to pass in the data to be encrypted with syntax ```--file [FILE]```.
Use the --source flag to pass in the original image file with syntax ```--source [IMAGE]```.
Use the --output flag to pass in the path to send the new image with syntax ```--output [IMAGE]```.

Together, the symtax for encrypting a file inside an image is as follows.
```
./StegSharp --file [FILE] --source [IMAGE] --output [FILE]
```

To encrypt a file named ```file.txt``` inside image ```image.png``` and save the result in
```image.e.png```, one would do the following:
```
./StegSharp --file file.txt --source image.png --output image.e.png
```

### Decrypting Data from an Encrypted Image

In order to decrypt the data stored inside an image, the encrypted image file and
original unencrypted image file must be passed on the command line, as well as an
output file for the resulting data.

Use the --source flag to pass in the original image file with syntax ```--file [IMAGE]```.
Use the --image flag to pass in the encrypted image file with symtax ```--image [IMAGE]```.
Use the --output flag to pass in the path to send the decrypted data with syntax ```--output [FILE]```.

Together, the symtax for decrypting a file inside an image is as follows.
```
./StegSharp --source [IMAGE] --image [IMAGE] --output [FILE]
```

To decrypt data inside an encrypted image named ```image.e.png``` with original image ```image.png```
and store the result in ```out.txt```, one would do the following:
```
./StegSharp --source image.png --image image.e.png --output out.txt
```

# Call of Duty Specular Image Name Fixer

This tool helps you to quickly fix the names of specular images for Call of Duty textures. It identifies specular maps (usually with keywords like `_spec`, `_spc`, or `_s` in the filename) and renames them according to a specific format, while also generating new cosine maps (`_cos`) based on the alpha channel. 
( Cosine generation is not correctly produced for Black Ops specular maps )

## Features:
- Automatically renames specular textures to match the required format.
- Generates `_cos` images based on the alpha channel from `_spc` files.
- Supports drag-and-drop functionality for easy use.

## Requirements:
- **.NET 9** runtime (if you're running the tool via the command line)
- Image files should be in formats like `.png`, `.dds`, `.tiff`, or `.tga`.

## Getting Started:

### 1. Download the Tool
- Download the tool from the release section.
- Extract the folder and keep it in an easily accessible location.

### 2. Prepare the Images
- Place all the specular images (which usually have names like `delta_alpha_a_spc.png`) into the folder named `_input`.
- If you don't have an `_input` folder, the program will create it automatically.

### 3. Run the Tool
#### Method 1: Drag-and-Drop
- Simply drag and drop your specular images onto the tool's `.exe` file to start processing.
- The tool will automatically move the fixed images to a folder called `_output`.

#### Method 2: Command Line
- If you prefer using the command line:
  - Open a terminal window in the folder where the tool is located.
  - Run the tool using: `dotnet SpecularNameFixer.dll` (if using .NET 9).
  - The program will ask you to place your images into the `_input` folder, and it will process them accordingly.

### 4. What Happens During Processing
- The tool will look for filenames containing keywords like `_spec`, `_spc`, or `_s`.
- It will rename these images to a simpler format (e.g., `delta_alpha_a_spc.png` becomes `delta_alpha_a.png`).
- A new file (`_cos.png`) will be generated, which contains the alpha channel extracted from the specular map.

### 5. Output
- After processing, the tool will save the renamed files and new cosine maps into the `_output` folder.
- Example:
  - `~delta_alpha_a_spc-rgb&delta_~155668f1.png` will be renamed to `delta_alpha_a_spc.png`.
  - A new cosine map will be generated and saved as `delta_alpha_a_cos.png`.

## Example Output:
File processed: ~delta_alpha_a_spc-rgb&delta_~155668f1.png -> delta_alpha_a_spc.png 
File processed: delta_alpha_a_spc.png -> delta_alpha_a_cos.png

## Troubleshooting:
- **No Input Folder:** If the `_input` folder doesn’t exist, the tool will create it for you.
- **No Files Found:** Ensure your specular images are named correctly with `_spec`, `_spc`, or `_s` in the filename and placed in the `_input` folder.
- **Errors during Processing:** If any file fails to process, the tool will display an error message with details.

## Contributing:
If you'd like to contribute to this tool or suggest improvements, feel free to create an issue or pull request on the GitHub repository.

## License:
This tool is open-source and free to use. You are welcome to modify and share it as needed.

## Contact:
If you need help or want to join our community, you can reach us through the following:
- Discord Server: [Join Here](https://discord.gg/GNMSApYh9y)
- Developed by: **NAKSHATRA_12**

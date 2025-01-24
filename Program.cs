using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace SpecularNameFixer
{
    public class Program
    {
        // Keywords for specular textures
        private static readonly string[] keywords = { "_spec", "_spc", "_s" };

        // Exclusions (to skip non-specular textures, only if they are at the end of the filename)
        private static readonly string[] exclusions = { "_col", "_cos", "_nml" , "_c" , "_n" };

        // Supported image extensions
        private static readonly string[] allowedExtensions = { ".png", ".dds", ".tiff", ".tga" };

        private static int specularConvertCount = 0, cosineConvertCount = 0;

        public static void Main(string[] args)
        {
            Console.WriteLine("Call of Duty Specular Images Name Fixer v1.0");
            Console.WriteLine("Tool Developed By - NAKSHATRA_12\n");
            Console.WriteLine("Discord Server - https://discord.gg/GNMSApYh9y\n");

            if (args.Length == 0)
                NonDragDropMethod();
            else
                DragDropMethod(args);
        }

        private static void DragDropMethod(string[] args)
        {
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "_output");

            // Create the _output folder if it doesn't exist
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            foreach (string filePath in args)
            {
                if (!File.Exists(filePath))
                {
                    continue;
                }

                string fileName = Path.GetFileName(filePath);
                string fileExtension = Path.GetExtension(filePath).ToLower();

                // Check if the file is an image file
                if (!IsImageFile(fileExtension))
                {
                    continue;
                }

                // Check if the file contains any of the exclusion keywords
                if (ContainsExclusions(fileName))
                {
                    continue;
                }

                // Check if the file contains any of the specular-related keywords
                if (!ContainsKeywords(fileName))
                {
                    continue;
                }

                // Apply transformation to the file name
                string transformedFileName = TransformFileName(fileName);

                // Log the transformation from original to the transformed name
                Console.WriteLine($"File processed: {fileName} -> {transformedFileName}");

                // Create the output path for the renamed `_spc` file
                string transformedSpcPath = Path.Combine(outputFolder, transformedFileName);

                try
                {
                    // Copy the file to the output folder with the transformed name
                    File.Copy(filePath, transformedSpcPath, true);
                    ++specularConvertCount;

                    // Generate the `_cos` file based on the alpha channel of `_spc`
                    GenerateCosFile(filePath, outputFolder, transformedFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {fileName}: {ex.Message}");
                }
            }

            Console.WriteLine($"\nProcessing complete! Total files converted: {specularConvertCount}");
            WaitForButtonPress();
        }

        private static void NonDragDropMethod()
        {
            Console.WriteLine("Place all the specular images in the _input folder to fix the names.");
            Console.WriteLine("You will get the fixed names of the images in the _output folder.\n");

            string inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "_input");
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "_output");

            // Create the _input and _output folders if they don't exist
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine("_input directory does not exist and has been created.\n" +
                                  "Place the images to be fixed in the _input folder and restart the application.");
                Directory.CreateDirectory(inputFolder);
                WaitForButtonPress();
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string[] files = Directory.GetFiles(inputFolder);

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string fileExtension = Path.GetExtension(file).ToLower();

                    // Check if the file is an image file
                    if (!IsImageFile(fileExtension))
                    {
                        continue;
                    }

                    // Check if the file contains any of the exclusion keywords
                    if (ContainsExclusions(fileName))
                    {
                        continue;
                    }

                    // Check if the file contains any of the specular-related keywords
                    if (!ContainsKeywords(fileName))
                    {
                        continue;
                    }

                    // Apply transformation to the file name
                    string transformedFileName = TransformFileName(fileName);

                    // Log the transformation from original to the transformed name
                    Console.WriteLine($"File processed: {fileName} -> {transformedFileName}");

                    // Create the output path for the renamed `_spc` file
                    string transformedSpcPath = Path.Combine(outputFolder, transformedFileName);

                    try
                    {
                        // Copy the file to the output folder with the transformed name
                        File.Copy(file, transformedSpcPath, true);
                        ++specularConvertCount;

                        // Generate the `_cos` file based on the alpha channel of `_spc`
                        GenerateCosFile(file, outputFolder, transformedFileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {fileName}: {ex.Message}");
                    }
                }

                Console.WriteLine($"\nProcessing complete!\nTotal specular converted: {specularConvertCount}");
                Console.WriteLine($"Total cosine images generated: {cosineConvertCount}");
            }
            else
            {
                Console.WriteLine("No files found in the '_input' folder.");
            }

            WaitForButtonPress();
        }

        private static void GenerateCosFile(string spcFilePath, string outputFolder, string spcFileName)
        {
            try
            {
                // Clean up the specular file name
                string baseName = Path.GetFileNameWithoutExtension(spcFileName);
                foreach (string keyword in keywords)
                {
                    baseName = Regex.Replace(baseName, Regex.Escape(keyword), "", RegexOptions.IgnoreCase);
                }
                baseName = baseName.TrimEnd('_'); // Remove trailing underscores

                // Prepare the name for the new `_cos` file
                string cosFileName = baseName + "_cos.png";

                // Log the transformation from the specular to cosine image
                //Console.WriteLine($"File processed: {spcFileName} -> {baseName + Path.GetExtension(spcFileName)}");

                using (Bitmap spcImage = new Bitmap(spcFilePath))
                {
                    // Create an empty image for the `_cos` output
                    Bitmap cosImage = new Bitmap(spcImage.Width, spcImage.Height);

                    for (int y = 0; y < spcImage.Height; y++)
                    {
                        for (int x = 0; x < spcImage.Width; x++)
                        {
                            Color pixel = spcImage.GetPixel(x, y);

                            // Extract the alpha channel and store it in grayscale
                            int alpha = pixel.A;
                            Color gray = Color.FromArgb(255, alpha, alpha, alpha);
                            cosImage.SetPixel(x, y, gray);
                        }
                    }

                    // Save the `_cos` file
                    string cosFilePath = Path.Combine(outputFolder, cosFileName);
                    cosImage.Save(cosFilePath, ImageFormat.Png);
                    cosImage.Dispose();

                    // Log the creation of the `_cos` file
                    Console.WriteLine($"File processed: {baseName + "_spc" + Path.GetExtension(spcFileName)} -> {cosFileName}\n");
                    ++cosineConvertCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating '_cos' file: {ex.Message}");
            }
        }

        // Helper methods
        private static bool IsImageFile(string extension)
        {
            return Array.Exists(allowedExtensions, ext => ext == extension);
        }

        private static bool ContainsExclusions(string fileName)
        {
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            foreach (string exclusion in exclusions)
            {
                if (Regex.IsMatch(nameWithoutExtension, Regex.Escape(exclusion), RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ContainsKeywords(string fileName)
        {
            foreach (string keyword in keywords)
            {
                if (Regex.IsMatch(fileName, Regex.Escape(keyword), RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static string TransformFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            foreach (string keyword in keywords)
            {
                int keywordIndex = nameWithoutExtension.LastIndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (keywordIndex != -1)
                {
                    nameWithoutExtension = nameWithoutExtension.Substring(0, keywordIndex + keyword.Length);
                    break;
                }
            }

            if (nameWithoutExtension.StartsWith("~"))
            {
                nameWithoutExtension = nameWithoutExtension.Substring(1);
            }

            return nameWithoutExtension + extension;
        }

        private static void WaitForButtonPress(bool showText = true, string textToPrint = "\nPress any key to exit...")
        {
            if (showText)
            {
                Console.WriteLine(textToPrint);
            }
            Console.ReadLine();
        }
    }
}
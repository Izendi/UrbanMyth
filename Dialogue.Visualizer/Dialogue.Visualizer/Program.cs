using System;
using System.Collections.Generic;
using System.IO;
using Dialogue.Models;
using Newtonsoft.Json;

namespace DialogueTreeGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path for the dialogue JSON:");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found!");
                return;
            }

            string jsonData = File.ReadAllText(filePath);
            DialogueModel dialogue = JsonConvert.DeserializeObject<DialogueModel>(jsonData);

            var path = Path.Combine(Path.GetDirectoryName(filePath), "Visualizations");

            var dotFilePath = Path.Combine(path, Path.GetFileNameWithoutExtension(filePath) + ".dot");

            // Generate dialogue graph in DOT format
            GenerateDotFile(dialogue.DialogueNodes, dotFilePath);
            Console.WriteLine($"DialogueModelgraph generated as {dotFilePath}");

            Console.WriteLine("\nSummary of Niceness Scores:");
            SummarizeNicenessScores(dialogue.DialogueNodes);
        }

        // Method to generate a DOT file for Graphviz visualization
        static string GenerateDotFile(List<DialogueNode> dialogueNodes, string dotFilePath)
        {
            //if (!Directory.Exists(directoryPath))
            //{
            //    Directory.CreateDirectory(directoryPath);
            //}

            // Save the .dot file in the dialogueplots folder

            using (StreamWriter writer = new StreamWriter(dotFilePath))
            {
                writer.WriteLine("digraph DialogueTree {");

                foreach (var node in dialogueNodes)
                {
                    // Escape quotes and other special characters in the dialogue text
                    string escapedText = EscapeGraphvizString(node.Text);
                    string nodeLabel = $"{node.DialogueId}: \"{escapedText}\"";

                    writer.WriteLine($"  {node.DialogueId} [label=\"{nodeLabel}\"];");

                    if (node.Responses != null && node.Responses.Count > 0)
                    {
                        foreach (var response in node.Responses)
                        {
                            // Escape the response text
                            string escapedResponse = EscapeGraphvizString(response.Text);
                            string edgeLabel = $"{escapedResponse} (Niceness: {response.ResponseNiceness})";

                            writer.WriteLine($"  {node.DialogueId} -> {response.NextDialogueId} [label=\"{edgeLabel}\"];");
                        }
                    }
                }

                writer.WriteLine("}");
            }

            return dotFilePath;

        }

        // Method to summarize niceness scores for ending nodes
        static void SummarizeNicenessScores(List<DialogueNode> dialogueNodes)
        {
            foreach (var node in dialogueNodes)
            {
                if (node.Responses == null || node.Responses.Count == 0)
                {
                    int totalNiceness = CalculateNicenessScore(node.DialogueId, dialogueNodes);
                    Console.WriteLine($"Ending Node {node.DialogueId}: Total Niceness Score = {totalNiceness}");
                }
            }
        }

        // Recursive method to calculate the total niceness score for a path
        static int CalculateNicenessScore(int dialogueId, List<DialogueNode> dialogueNodes)
        {
            DialogueNode node = dialogueNodes.Find(n => n.DialogueId == dialogueId);
            if (node == null || node.Responses == null || node.Responses.Count == 0)
            {
                return 0; // End node
            }

            int nicenessScore = 0;
            foreach (var response in node.Responses)
            {
                nicenessScore += response.ResponseNiceness +
                                 CalculateNicenessScore(response.NextDialogueId, dialogueNodes);
            }

            return nicenessScore;
        }

        static string EscapeGraphvizString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Escape quotes and backslashes
            input = input.Replace("\\", "\\\\"); // Escape backslashes
            input = input.Replace("\"", "\\\""); // Escape quotes

            return input;
        }
    }
}
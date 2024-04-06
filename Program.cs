using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

internal class Program
{
    private static void Main(string[] args)
    {
        BrainFuck bf = new();
        Console.WriteLine("Enter your brainfuck code:");
        bf.ParseBrainFuckCommands(Console.ReadLine());
        bf.printMemory();
    }

    private class BrainFuck
    {
        private int pointer = 0;
        private List<int> memory = new();

        public void clearMem()
        {
            memory.Clear();
        }

        public void resetPointer()
            { pointer = 0; }

        public void resetAll()
        {
            clearMem();
            resetPointer();
        }

        public int getPointer() { return pointer; }

        public void printMemory()
        {
            foreach(int cell in memory)
            {
                Console.Write("[");
                Console.Write(cell);
                Console.Write("]");
            }
            Console.WriteLine();
            for(int i = 0; i < pointer; i++)
            {
                Console.Write("   ");
            }
            Console.Write(" ^");
            Console.WriteLine();
        }

        private int? ParseCommand(char command, List<int> memory, int cell)
        {
            while (cell > memory.Count - 1)
            {
                memory.Add(0);
            }
            switch (command)
            {
                case '+':
                    memory[cell] += 1;
                    break;
                case '-':
                    memory[cell] -= 1;
                    break;
                case '>':
                    return cell += 1;
                case '<':
                    return cell -= 1; ;
                case '.':
                    Console.WriteLine(memory[cell].ToString() + " - <" + (char)memory[cell] + ">");
                    break;
                case ',':
                    try
                    {
                        memory[cell] = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        break;
                    }
                default:
                    Console.WriteLine("[Warning]: Skipped unknown command \"" + command + "\"");
                    return null;
            }
            return null;
        }


        private string getLoop(string commands, int index)
        {
            int depth = 0;
            for(int i = index; i<commands.Length; i++)
            {
                char character = commands[i+1];
                if(character == '[')
                {
                    depth++;
                }
                if(character == ']' && depth == 0)
                {
                    int startIndex = index;
                    int endIndex = i;
                    int length = endIndex - startIndex + 1;
                    return commands.Substring(startIndex, length)[1..];
                }
                if(character == ']')
                {
                    depth--;
                }
            }
            throw new Exception("[ was never closed");
        }
        private void executeLoop(string loop)
        {
            while (memory[pointer] != 0)
            {
                ParseBrainFuckCommands(loop);
            }
        }

        public List<int> ParseBrainFuckCommands(string commands)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                char command = commands[i];
                if (!(command == '[' || command == ']'))
                {
                    int? result = ParseCommand(command, memory, pointer);
                    if (result != null)
                    {
                        pointer = result.Value;
                    }
                }
                else
                {
                    if (command == '[')
                    {
                        string loop = getLoop(commands, i);
                        executeLoop(loop);
                        i += loop.Length;
                    }
                }
            }
            return memory;
        }
    }
}

namespace Day11;

public class Blink(string input)
{
    private LinkedList<long> stones = new(Array.ConvertAll(input.Split(), long.Parse));

    public int PartOne(int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            BlinkTimes();
        }

        checked
        {
            return stones.Count;
        }
    }

    private void BlinkTimes()
    {
        var currentNode = stones.First;
        while (currentNode is not null)
        {
            var nextNode = currentNode.Next;

            switch (currentNode.Value)
            {
                case 0:
                    currentNode.Value = 1;
                    break;
                case var n when HasEvenLength(n):
                    var (left, right) = SplitStone(n);

                    stones.AddBefore(currentNode, left);
                    stones.AddAfter(currentNode, right);
                    stones.Remove(currentNode);
                    break;
                default:
                    currentNode.Value *= 2024;
                    break;
            }

            currentNode = nextNode;
        }
    }

    private static long Divisor(long num) => (long)Math.Pow(10, num.ToString().Length / 2);

    private static bool HasEvenLength(long num) => num.ToString().Length % 2 == 0;

    private static (long, long) SplitStone(long num)
    {
        long divisor = Divisor(num);
        return (num / divisor, num % divisor);
    }
}

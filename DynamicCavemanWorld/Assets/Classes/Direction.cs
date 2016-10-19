// using nothing

public class Direction {

    public string direction;

    public Direction(string direction)
    {
        this.direction = direction;
    }

    public float getFloatAtCoordinates(int x, int z, float[,] array)
    {
        int a = 0;
        int b = 0;
        switch (direction)
        {
            case "up":
                a = x;
                b = z - 1;
                break;
            case "down":
                a = x;
                b = z + 1;
                break;
            case "left":
                a = x - 1;
                b = z;
                break;
            case "right":
                a = x + 1;
                b = z;
                break;
        }

        return array[a, b];
    }
}

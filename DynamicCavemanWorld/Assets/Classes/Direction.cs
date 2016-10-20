// using nothing

public class Direction {

    public string direction;

    public Direction(string direction)
    {
        this.direction = direction;
    }

    public float getFloatAtCoordinates(int x, int z, float[,] array)
    {
        int[] coor = getCoordinateArray(x, z);
        return array[coor[0], coor[1]];
    }

    public int[] getCoordinateArray(int x, int z)
    {
        int[] coor = new int[2];
        switch (direction)
        {
            case "up":
                coor[0] = x;
                coor[1] = z - 1;
                break;
            case "down":
                coor[0] = x;
                coor[1] = z + 1;
                break;
            case "left":
                coor[0] = x - 1;
                coor[1] = z;
                break;
            case "right":
                coor[0] = x + 1;
                coor[1] = z;
                break;
        }

        return coor;
    }
}

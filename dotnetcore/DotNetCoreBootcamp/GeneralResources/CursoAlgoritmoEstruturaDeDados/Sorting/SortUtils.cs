namespace GeneralResources.CursoAlgoritmoEstruturaDeDados.Sorting
{
    public class SortUtils
    {
        public static void Swap(IList<int> m, int x, int y)
        {
            int temp = m[y];
            m[y] = m[x];
            m[x] = temp;
        }
    }
}

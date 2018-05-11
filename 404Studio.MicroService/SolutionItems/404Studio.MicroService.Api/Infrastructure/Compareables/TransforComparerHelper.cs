namespace YH.Etms.Settlement.Api.Infrastructure.Compareables
{
    public class TransforComparerHelper
    {
        //public static (IEnumerable<TEntity> sames,IEnumerable<TEntity> differents,IEnumerable<TEntity> subs) Comparer<TEntity>(List<TEntity> orign,List<TEntity> second) where TEntity:class
        //{
        //    var compare = new TransforEqualityComparer<TEntity>();
        //    //取交集
        //    var intersect = orign.Intersect(second, compare);
        //    //取差集
        //    var except = orign.Except(second, compare);
        //    if (intersect.Any())
        //    {

        //    }
        //    else
        //    {
        //        //没有交集就判断长度
        //        if (orign.Count == second.Count)
        //        {
        //            if(intersect.Count() == orign.Count)
        //            {
        //                return (Enumerable.Empty<TEntity>(), orign, Enumerable.Empty<TEntity>());
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    //var same = orign.Intersect(compared, compare);
        //    //var different = orign.Except(compared, compare);
        //    //IEnumerable<TEntity> subs = Enumerable.Empty<TEntity>();
        //    //if(orign.Count < compared.Count)
        //    //{
        //    //    var adds = compared.Except(orign);//新增集合
        //    //}
        //    //if(orign.Count > compared.Count)
        //    //{
        //    //    subs = compared.Except(orign, compare);
        //    //}
        //    //if(orign.Count == compared.Count)
        //    //{

        //    //}
        //    return (same, different, subs);
        //}
    }
}

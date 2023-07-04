using DataAccessLayer.Contracts;

namespace BusinessLogicLayer.Extensions     
{
    public static class ProjectFilter
    {
        public static IQueryable<TEntity> FilterByProjectId<TEntity>(this IQueryable<TEntity> query, int projectId)
        where TEntity : IMustHaveProject
        {
            return query.Where(entity => entity.ProjectID == projectId);
        }
    }
}

using Bramf.Operation;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Bramf.Patterns.Repository.Mongo
{
    /// <summary>
    /// Inserts an entity
    /// </summary>
    internal class InsertOperation<TEntity> : IOperation
        where TEntity : IEntity
    {
        private TEntity mEntity;
        private IMongoCollection<TEntity> mCollection;

        public InsertOperation(TEntity entity, IMongoCollection<TEntity> collection)
        {
            mEntity = entity;
            mCollection = collection;
        }

        public async Task<OperationResult> ExecuteAsync()
        {
            try
            {
                await mCollection.InsertOneAsync(mEntity);
                return OperationResult.Success;
            }
            catch(Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }
    }

    /// <summary>
    /// Inserts an entity
    /// </summary>
    internal class RemoveOperation<TEntity> : IOperation
        where TEntity : IEntity
    {
        private TEntity mEntity;
        private IMongoCollection<TEntity> mCollection;

        public RemoveOperation(TEntity entity, IMongoCollection<TEntity> collection)
        {
            mEntity = entity;
            mCollection = collection;
        }

        public async Task<OperationResult> ExecuteAsync()
        {
            object entityId = mEntity.Id;

            try
            {
                await mCollection.DeleteOneAsync(x => x.Id == entityId);
                return OperationResult.Success;
            }
            catch(Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }
    }

    /// <summary>
    /// Inserts an entity
    /// </summary>
    internal class UpdateOperation<TEntity> : IOperation
        where TEntity : IEntity
    {
        private TEntity mEntity;
        private IMongoCollection<TEntity> mCollection;

        public UpdateOperation(TEntity entity, IMongoCollection<TEntity> collection)
        {
            mEntity = entity;
            mCollection = collection;
        }

        public async Task<OperationResult> ExecuteAsync()
        {
            object entityId = mEntity.Id;

            try
            {
                await mCollection.ReplaceOneAsync(x => x.Id == entityId, mEntity);
                return OperationResult.Success;
            }
            catch(Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }
    }
}
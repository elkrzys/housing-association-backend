using System;
using System.Transactions;

namespace HousingAssociation.DataAccess.Context
{
    public static class TransactionHandler
    {
        public static void RunInSingleTransactionAndReturnVoid(Action functionToInvoke)
        {
            using var scope = new TransactionScope();
            functionToInvoke();
            scope.Complete();
        }
        public static T RunInSingleTransactionAndReturnValue<T>(Func<T> functionToInvoke)
        {
            using var scope = new TransactionScope();
            var result = functionToInvoke();
            scope.Complete();
            return result;
        }
    }
}

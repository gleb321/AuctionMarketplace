using System;
using Npgsql;

namespace AuctionService.Tools {
    public static class NpgsqlCommandExtensions {
        public static void FillParameters<T>(this NpgsqlCommand command, T[] parameters, string typeShortcut) where T: notnull {
            for (int i = 0; i < parameters.Length; ++i) {
                command.Parameters.AddWithValue($"{typeShortcut}{i}", parameters[i]);
            }
        }
    }
}
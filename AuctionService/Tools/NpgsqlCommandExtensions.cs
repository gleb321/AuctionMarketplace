using System;
using Npgsql;

namespace AuctionService.Tools {
    public static class NpgsqlCommandExtensions {
        public static void FillStringParameters(this NpgsqlCommand command, string[] parameters) {
            for (int i = 0; i < parameters.Length; ++i) {
                command.Parameters.AddWithValue($"str{i}", parameters[i]);
            }
        }

        public static void FillDateTimeParameters(this NpgsqlCommand command, DateTime[] parameters) {
            for (int i = 0; i < parameters.Length; ++i) {
                command.Parameters.AddWithValue($"dt{i}", parameters[i]);
            }
        }
    }
}
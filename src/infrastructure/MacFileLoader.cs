using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Filesync
{
  class MacFileLoader : IFileLoader
  {
    private async Task<List<Substitution>> loadSubstitutionsFromConnection(SqliteConnection connection)
    {
      var command = connection.CreateCommand();
      command.CommandText = "SELECT zphrase, zshortcut FROM ZUSERDICTIONARYENTRY";
      var substitutions = new List<Substitution>();
      using (var reader = await command.ExecuteReaderAsync())
      {
        while (reader.Read())
        {
          var word = reader.GetString(0);
          var shortcut = reader.GetString(1);
          substitutions.Add(new Substitution
          {
            Shortcut = shortcut,
            Word = word
          });
        }
      }
      return substitutions;
    }

    private async Task addSubstitutionToConnection(SqliteConnection connection, Substitution addedSubstitution)
    {
      var command = connection.CreateCommand();
      command.CommandText = $@"INSERT 
                              INTO ZUSERDICTIONARYENTRY 
                              (Z_ENT, Z_OPT, ZTIMESTAMP, ZPHRASE, ZSHORTCUT) 
                              VALUES (1, 1, 624393238, '{addedSubstitution.Word}', '{addedSubstitution.Shortcut}')";
      await command.ExecuteNonQueryAsync();
    }

    private async Task removeSubstitutionToConnection(SqliteConnection connection, Substitution removedSubstitution)
    {
      var command = connection.CreateCommand();
      command.CommandText = $@"DELETE 
                              FROM ZUSERDICTIONARYENTRY 
                              WHERE ZSHORTCUT = '{removedSubstitution.Shortcut}'";
      await command.ExecuteNonQueryAsync();
    }

    private async Task updateSubstitutionToConnection(SqliteConnection connection, Substitution updatedSubstitution)
    {
      var command = connection.CreateCommand();
      command.CommandText = $@"UPDATE ZUSERDICTIONARYENTRY
                              SET ZPHRASE = '{updatedSubstitution.Word}'
                              WHERE ZSHORTCUT = '{updatedSubstitution.Shortcut}'";
      await command.ExecuteNonQueryAsync();
    }


    public async Task<Substitutions> loadFileAsync(string path)
    {
      Console.WriteLine("Reading Mac");
      using (var connection = GetConnection(path))
      {
        await connection.OpenAsync();
        var substitutions = await loadSubstitutionsFromConnection(connection);
        return new Substitutions
        {
          substitutions = substitutions
        };
      }
    }

    private static SqliteConnection GetConnection(string path)
    {
      return new SqliteConnection($"Data Source={path}");
    }

    public async Task saveFileAsync(Substitutions newSubstitutions, string path)
    {
      Console.WriteLine("Writing Mac");

      using (var connection = GetConnection(path))
      {
        await connection.OpenAsync();
        var existingSubstitutions = await loadSubstitutionsFromConnection(connection);
        var addedSubs = SubstitutionsComparator.FindAddedSubstitutions(existingSubstitutions, newSubstitutions.substitutions);
        var deletedSubs = SubstitutionsComparator.FindDeletedSubstitutions(existingSubstitutions, newSubstitutions.substitutions);
        var modifiedSubs = SubstitutionsComparator.FindModifiedSubstitutions(existingSubstitutions, newSubstitutions.substitutions);
        addedSubs.ToList().ForEach(async addedSub => await this.addSubstitutionToConnection(connection, addedSub));
        deletedSubs.ToList().ForEach(async deletedSub => await this.removeSubstitutionToConnection(connection, deletedSub));
        modifiedSubs.ToList().ForEach(async modifiedSub => await this.updateSubstitutionToConnection(connection, modifiedSub));
      }
    }
  }
}
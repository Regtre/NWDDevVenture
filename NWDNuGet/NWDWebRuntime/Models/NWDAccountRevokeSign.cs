using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NWDWebRuntime.Models;

[Serializable]
public class NWDAccountRevokeSign
{
    [ValidateNever] 
    public string SignName { set; get; } = string.Empty;
    public ulong Reference { set; get; }
}
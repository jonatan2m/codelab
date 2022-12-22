using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample
{
    /// <summary>
    /// For testing internal classes, we need to decorate them with an attribute, InternalsVisibleTo.
    /// There are few ways to do that but the most useful is set it in AssemblyInfo.cs file ou put it in .csproj
    /// 
    /// 1 - On folder Properties, create a file AssemblyInfo.cs and insert this piece of code
    /// <code>
    /// using System.Runtime.CompilerServices;
    /// 
    /// [assembly: InternalsVisibleTo("test project name")]
    /// </code>
    /// If you have a public key, you can set it as a parameter
    /// <code>
    /// [assembly: InternalsVisibleTo("test project name, PublicKey=adfdasfdasf")]
    /// </code>
    /// 
    /// <ItemGroup>
    ///     <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute">
    ///         <_Parameter1>GeneralResources</_Parameter1>
    ///     </AssemblyAttribute>
    /// </ItemGroup>
    /// 
    /// Or using variables, like $(AssemblyName)
    /// <ItemGroup>
    ///     <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute">
    ///         <_Parameter1>$(AssemblyName).Tests</_Parameter1>
    ///     </AssemblyAttribute>
    /// </ItemGroup>
    /// 
    /// Since .NET 5, we need just put this configuration:
    /// <ItemGroup>
    ///     <InternalsVisibleTo Include="test project name" />
    /// </ItemGroup>
    /// </summary>
    internal class TestInternalClasses
    {
    }
}

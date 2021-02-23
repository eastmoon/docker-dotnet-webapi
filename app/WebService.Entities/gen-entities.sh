# Declare variable
connectionString='Server=mysql;User Id=root;Password=rootps;Database=demo_db;Port=3306;TreatTinyAsBoolean=true;'
connectionLibrary='Pomelo.EntityFrameworkCore.MySql'

# Declare function
function gen() {
    contextName=${1}
    echo "Generate ${contextName} by dotnet EntityFrameworkCore tools"
    # Generate entities with dotnet EntityFrameworkCore tools
    dotnet ef dbcontext scaffold "${connectionString}" "${connectionLibrary}" -f -c ${contextName} --context-dir Context -o Models
    # Remove unsafe code
    sed -i '/#warning/d' ./Context/${contextName}.cs
    sed -i '/optionsBuilder.UseMySql/d' ./Context/${contextName}.cs
}

# Execute script
[ -d Context ] && rm -rf Context
mkdir Context
[ -d Models ] && rm -rf Models
mkdir Models
gen CommandDBContext
gen QueryDBContext

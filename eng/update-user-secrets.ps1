$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Path | Join-Path -ChildPath ".."

$envFilePath = $rootPath 
| Join-Path -ChildPath ".env" 
| Resolve-Path

$projectPath = $rootPath 
| Join-Path -ChildPath "components/service/src/FluxoCaixa.WebApi/FluxoCaixa.WebApi.csproj" 
| Resolve-Path

if (Test-Path $envFilePath) {
    $connectionString = "mongodb://<username>:<password>@localhost:27017/";

    foreach ($line in Get-Content $envFilePath) {
        # Skip empty lines and comments
        if (-not [string]::IsNullOrWhiteSpace($line) -and $line -notlike "#*") {
            # Split the line into key and value
            $parts = $line -split "=", 2
            $name = $parts[0].Trim()
            $value = $parts[1].Trim()

            if ($name -eq "MONGODB_USERNAME") {
                $connectionString = $connectionString -replace "<username>", $value
            }

            if ($name -eq "MONGODB_PASSWORD") {
                $connectionString = $connectionString -replace "<password>", $value
            }
        }
    }

    dotnet user-secrets --project $projectPath set "ConnectionStrings:MongoDB" $connectionString
}
else {
    Write-Warning "Você não tem um arquivo .env. Esperamos um '$envFilePath'."
}
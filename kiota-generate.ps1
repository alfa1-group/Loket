# Kiota generation script for Loket API client in C#

$buildersGeneratedPath = "src/Loket.Api.Client/Generated/Api/V2"
$modelsPath = "./src/Loket.Api.Client/Generated/Models"
$extensionsModelsPath = "./src/Loket.Api.Client/Extensions/Models"

Write-Output "⚙️ Generating C# client code..."
kiota generate `
    --cc `
    --ad false `
    --openapi "./resources/Loket-openapi.json" `
    --clean-output `
    --language CSharp `
    --output "./src/Loket.Api.Client/Generated" `
    --namespace-name "Loket.Api.Client" `
    --class-name "LoketServiceClient" `

Write-Output "✅ Classes generation completed"
$SourceFolder = "C:\Users\Jos\source\repos\mar\vss\marVSS\marVSS"
$ZipFile = "C:\Users\Jos\source\repos\mar\vss\marVSS\marVSS.zip"

Write-Host "Starting Defender scan..." -ForegroundColor Yellow

Start-MpScan -ScanType CustomScan -ScanPath $SourceFolder

# Wait a few seconds for Defender to register results
Start-Sleep -Seconds 10

$Threats = Get-MpThreatDetection |
Where-Object { $_.Resources -match [regex]::Escape($SourceFolder) }

if ($Threats) {
    Write-Host "Threat detected. ZIP not created." -ForegroundColor Red
    $Threats | Format-Table ThreatName, Resources
    exit 1
}

Write-Host "Folder is clean." -ForegroundColor Green

if (Test-Path $ZipFile) {
    Remove-Item $ZipFile -Force
}

Compress-Archive `
    -Path "$SourceFolder\*" `
    -DestinationPath $ZipFile `
    -CompressionLevel Optimal

Write-Host "ZIP created: $ZipFile" -ForegroundColor Green

Write-Host ""
Write-Host "Einde van Script. Druk Enter..."
Read-Host

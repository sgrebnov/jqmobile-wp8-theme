param($installPath, $toolsPath, $package, $project)

$project = Get-Project;

function Get-Type($kind) {
    switch($kind) {
        '{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}' { 'File' }
        '{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}' { 'Folder' }
        default { $kind }
    }
}
		
Function FixBuildAction
{
    param ($file)
	
	$prop = $file.Properties.Item("BuildAction");
    if ($prop -ne $null) {
		$prop.Value = [int]2;
	}
}

Function IterateFolder
{
	param ($item)
	
	#Write-Host "Processing `"$($item.Name)`""
	
	if(Get-Type $item.Kind -eq 'Folder') {
	
		foreach ($subItem in $item.ProjectItems)
		{   
		   IterateFolder $subItem;
		}                            
    }
	
	if(Get-Type $item.Kind -eq 'File') {
		
		if ($item.Name.ToUpper().EndsWith(".PNG") -or $item.Name.ToUpper().EndsWith(".JPG"))
		{
			#Write-Host $item.Name
			
			FixBuildAction $item;
		}
	}	
}

Write-Host "Updating images in www\images. It may take some time..."
IterateFolder $project.ProjectItems.item("www").ProjectItems.item("images");

Write-Host "Updating images in www\css\wp\images. It may take some time..."
IterateFolder $project.ProjectItems.item("www").ProjectItems.item("css").ProjectItems.item("wp").ProjectItems.item("images");

Write-Host "Updating images in www\css\default\images. It may take some time..."
IterateFolder $project.ProjectItems.item("www").ProjectItems.item("css").ProjectItems.item("default").ProjectItems.item("images");

Write-Host "Done!"
	

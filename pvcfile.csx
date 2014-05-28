pvc.Task("nuget-push", () => {
    pvc.Source("src/Pvc.Concat.csproj")
       .Pipe(new PvcNuGetPack(
            createSymbolsPackage: true
       ))
       .Pipe(new PvcNuGetPush());
});

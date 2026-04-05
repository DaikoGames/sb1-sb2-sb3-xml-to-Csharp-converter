
const fs = require('fs/promises');
const path = require('path');
const VirtualMachine = require('scratch-vm');
const { ScratchStorage } = require('scratch-storage');

(async () => {
    const vm = new VirtualMachine();

    // Attach storage so assets load properly
    const storage = new ScratchStorage();
    vm.attachStorage(storage);

    try {
        const SB1File = await fs.readFile('project.sb')
        await vm.loadProject(SB1File);
        const sb3Blob = await vm.saveProjectSb3();
        const arrayBuffer = await sb3Blob.arrayBuffer();
        const buffer = Buffer.from(arrayBuffer);
        await fs.writeFile('project.sb3', buffer);
    } 
    catch{
        try {
            const SB2File = await fs.readFile('project.sb2')
            await vm.loadProject(SB2File);
            const sb3Blob = await vm.saveProjectSb3();
            const arrayBuffer = await sb3Blob.arrayBuffer();
            const buffer = Buffer.from(arrayBuffer);
            fs.writeFile('project.sb3', buffer);
        }
        catch{
            //No Scratch File
        }
    }

})();
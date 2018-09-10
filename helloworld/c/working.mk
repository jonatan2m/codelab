##
## Auto Generated makefile by CodeLite IDE
## any manual changes will be erased      
##
## Debug
ProjectName            :=working
ConfigurationName      :=Debug
WorkspacePath          :=C:/Projects/codelab/helloworld/c
ProjectPath            :=C:/Projects/codelab/helloworld/c
IntermediateDirectory  :=./Debug
OutDir                 := $(IntermediateDirectory)
CurrentFileName        :=
CurrentFilePath        :=
CurrentFileFullPath    :=
User                   :=Jonatan
Date                   :=10/09/2018
CodeLitePath           :="C:/Program Files/CodeLite"
LinkerName             :=C:/TDM-GCC-64/bin/g++.exe
SharedObjectLinkerName :=C:/TDM-GCC-64/bin/g++.exe -shared -fPIC
ObjectSuffix           :=.o
DependSuffix           :=.o.d
PreprocessSuffix       :=.i
DebugSwitch            :=-g 
IncludeSwitch          :=-I
LibrarySwitch          :=-l
OutputSwitch           :=-o 
LibraryPathSwitch      :=-L
PreprocessorSwitch     :=-D
SourceSwitch           :=-c 
OutputFile             :=$(IntermediateDirectory)/$(ProjectName)
Preprocessors          :=
ObjectSwitch           :=-o 
ArchiveOutputSwitch    := 
PreprocessOnlySwitch   :=-E
ObjectsFileList        :="working.txt"
PCHCompileFlags        :=
MakeDirCommand         :=makedir
RcCmpOptions           := 
RcCompilerName         :=C:/TDM-GCC-64/bin/windres.exe
LinkOptions            :=  
IncludePath            :=  $(IncludeSwitch). $(IncludeSwitch). 
IncludePCH             := 
RcIncludePath          := 
Libs                   := 
ArLibs                 :=  
LibPath                := $(LibraryPathSwitch). 

##
## Common variables
## AR, CXX, CC, AS, CXXFLAGS and CFLAGS can be overriden using an environment variables
##
AR       := C:/TDM-GCC-64/bin/ar.exe rcu
CXX      := C:/TDM-GCC-64/bin/g++.exe
CC       := C:/TDM-GCC-64/bin/gcc.exe
CXXFLAGS :=  -g -O0 -Wall $(Preprocessors)
CFLAGS   :=  -g -O0 -Wall $(Preprocessors)
ASFLAGS  := 
AS       := C:/TDM-GCC-64/bin/as.exe


##
## User defined environment variables
##
CodeLiteDir:=C:\Program Files\CodeLite
Objects0=$(IntermediateDirectory)/pointer.c$(ObjectSuffix) $(IntermediateDirectory)/queue.c$(ObjectSuffix) $(IntermediateDirectory)/binaryheap.c$(ObjectSuffix) $(IntermediateDirectory)/src_binarysearchtree.c$(ObjectSuffix) $(IntermediateDirectory)/src_hashtable.c$(ObjectSuffix) $(IntermediateDirectory)/main.c$(ObjectSuffix) $(IntermediateDirectory)/linkedlist.c$(ObjectSuffix) $(IntermediateDirectory)/utils.c$(ObjectSuffix) $(IntermediateDirectory)/array.c$(ObjectSuffix) 



Objects=$(Objects0) 

##
## Main Build Targets 
##
.PHONY: all clean PreBuild PrePreBuild PostBuild MakeIntermediateDirs
all: $(OutputFile)

$(OutputFile): $(IntermediateDirectory)/.d $(Objects) 
	@$(MakeDirCommand) $(@D)
	@echo "" > $(IntermediateDirectory)/.d
	@echo $(Objects0)  > $(ObjectsFileList)
	$(LinkerName) $(OutputSwitch)$(OutputFile) @$(ObjectsFileList) $(LibPath) $(Libs) $(LinkOptions)

MakeIntermediateDirs:
	@$(MakeDirCommand) "./Debug"


$(IntermediateDirectory)/.d:
	@$(MakeDirCommand) "./Debug"

PreBuild:


##
## Objects
##
$(IntermediateDirectory)/pointer.c$(ObjectSuffix): pointer.c $(IntermediateDirectory)/pointer.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/pointer.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/pointer.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/pointer.c$(DependSuffix): pointer.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/pointer.c$(ObjectSuffix) -MF$(IntermediateDirectory)/pointer.c$(DependSuffix) -MM pointer.c

$(IntermediateDirectory)/pointer.c$(PreprocessSuffix): pointer.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/pointer.c$(PreprocessSuffix) pointer.c

$(IntermediateDirectory)/queue.c$(ObjectSuffix): queue.c $(IntermediateDirectory)/queue.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/queue.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/queue.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/queue.c$(DependSuffix): queue.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/queue.c$(ObjectSuffix) -MF$(IntermediateDirectory)/queue.c$(DependSuffix) -MM queue.c

$(IntermediateDirectory)/queue.c$(PreprocessSuffix): queue.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/queue.c$(PreprocessSuffix) queue.c

$(IntermediateDirectory)/binaryheap.c$(ObjectSuffix): binaryheap.c $(IntermediateDirectory)/binaryheap.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/binaryheap.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/binaryheap.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/binaryheap.c$(DependSuffix): binaryheap.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/binaryheap.c$(ObjectSuffix) -MF$(IntermediateDirectory)/binaryheap.c$(DependSuffix) -MM binaryheap.c

$(IntermediateDirectory)/binaryheap.c$(PreprocessSuffix): binaryheap.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/binaryheap.c$(PreprocessSuffix) binaryheap.c

$(IntermediateDirectory)/src_binarysearchtree.c$(ObjectSuffix): src/binarysearchtree.c $(IntermediateDirectory)/src_binarysearchtree.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/src/binarysearchtree.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/src_binarysearchtree.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/src_binarysearchtree.c$(DependSuffix): src/binarysearchtree.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/src_binarysearchtree.c$(ObjectSuffix) -MF$(IntermediateDirectory)/src_binarysearchtree.c$(DependSuffix) -MM src/binarysearchtree.c

$(IntermediateDirectory)/src_binarysearchtree.c$(PreprocessSuffix): src/binarysearchtree.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/src_binarysearchtree.c$(PreprocessSuffix) src/binarysearchtree.c

$(IntermediateDirectory)/src_hashtable.c$(ObjectSuffix): src/hashtable.c $(IntermediateDirectory)/src_hashtable.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/src/hashtable.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/src_hashtable.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/src_hashtable.c$(DependSuffix): src/hashtable.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/src_hashtable.c$(ObjectSuffix) -MF$(IntermediateDirectory)/src_hashtable.c$(DependSuffix) -MM src/hashtable.c

$(IntermediateDirectory)/src_hashtable.c$(PreprocessSuffix): src/hashtable.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/src_hashtable.c$(PreprocessSuffix) src/hashtable.c

$(IntermediateDirectory)/main.c$(ObjectSuffix): main.c $(IntermediateDirectory)/main.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/main.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/main.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/main.c$(DependSuffix): main.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/main.c$(ObjectSuffix) -MF$(IntermediateDirectory)/main.c$(DependSuffix) -MM main.c

$(IntermediateDirectory)/main.c$(PreprocessSuffix): main.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/main.c$(PreprocessSuffix) main.c

$(IntermediateDirectory)/linkedlist.c$(ObjectSuffix): linkedlist.c $(IntermediateDirectory)/linkedlist.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/linkedlist.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/linkedlist.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/linkedlist.c$(DependSuffix): linkedlist.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/linkedlist.c$(ObjectSuffix) -MF$(IntermediateDirectory)/linkedlist.c$(DependSuffix) -MM linkedlist.c

$(IntermediateDirectory)/linkedlist.c$(PreprocessSuffix): linkedlist.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/linkedlist.c$(PreprocessSuffix) linkedlist.c

$(IntermediateDirectory)/utils.c$(ObjectSuffix): utils.c $(IntermediateDirectory)/utils.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/utils.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/utils.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/utils.c$(DependSuffix): utils.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/utils.c$(ObjectSuffix) -MF$(IntermediateDirectory)/utils.c$(DependSuffix) -MM utils.c

$(IntermediateDirectory)/utils.c$(PreprocessSuffix): utils.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/utils.c$(PreprocessSuffix) utils.c

$(IntermediateDirectory)/array.c$(ObjectSuffix): array.c $(IntermediateDirectory)/array.c$(DependSuffix)
	$(CC) $(SourceSwitch) "C:/Projects/codelab/helloworld/c/array.c" $(CFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/array.c$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/array.c$(DependSuffix): array.c
	@$(CC) $(CFLAGS) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/array.c$(ObjectSuffix) -MF$(IntermediateDirectory)/array.c$(DependSuffix) -MM array.c

$(IntermediateDirectory)/array.c$(PreprocessSuffix): array.c
	$(CC) $(CFLAGS) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/array.c$(PreprocessSuffix) array.c


-include $(IntermediateDirectory)/*$(DependSuffix)
##
## Clean
##
clean:
	$(RM) -r ./Debug/



require 'albacore'

@@xml_coverage = "codecoverage/coverage.xml"
@@coverage_report = "codecoverage/report/"
@@output_folder = File.join(File.dirname(__FILE__), "build")

task :default => :coveragereport

desc "Generate version information"
Albacore::AssemblyInfoTask.new(:versioninfo) do |asm|
	asm.version = "0.1.0.0"
	asm.copyright = "Copyright (C)2009 Derick Bailey. All Rights Reserved."
	asm.output_file = "src/VersionInfo.cs"
end

desc "Build the UnitOfWork solution"
Albacore::MSBuildTask.new(:build => :versioninfo) do |msb|
	msb.log_level = :verbose
	msb.properties = {:configuration => :Release, :OutputPath => @@output_folder.gsub("/", "\\")}
	msb.targets [:Clean, :Build]
	msb.solution = "src/UoW.sln"
end

desc "Run code coverage analysis"
Albacore::NCoverConsoleTask.new(:coverageanalysis => :build) do |ncc|
	ncc.log_level = :verbose
	ncc.path_to_command = "tools/NCover-v3.3/NCover.Console.exe"
	ncc.output = {:xml => @@xml_coverage}
	ncc.working_directory = "build/"
	
	nunit = NUnitTestRunner.new("tools/NUnit-v2.5/nunit-console.exe")
	nunit.log_level = :verbose
	nunit.assemblies << "UoW.Specs.dll"
	nunit.options << '/noshadow'
	
	ncc.testrunner = nunit
end	

desc "Run code coverage report"
Albacore::NCoverReportTask.new(:coveragereport => :coverageanalysis) do |ncr|
	ncr.path_to_command = "tools/NCover-v3.3/NCover.Reporting.exe"
	ncr.coverage_files << @@xml_coverage
	
	fullcoveragereport = NCover::FullCoverageReport.new
	fullcoveragereport.output_path = @@coverage_report
	ncr.reports << fullcoveragereport
	
	ncr.required_coverage << NCover::BranchCoverage.new(:minimum => 75)
	ncr.required_coverage << NCover::SymbolCoverage.new(:minimum => 80)
	ncr.required_coverage << NCover::MethodCoverage.new(:minimum => 80)
	ncr.required_coverage << NCover::CyclomaticComplexity.new(:maximum => 20, :item_type => :Class)
	
	ncr.filters << NCover::AssemblyFilter.new(:filter_type => :include, :filter => "UoW*")
	ncr.filters << NCover::AssemblyFilter.new(:filter_type => :exclude, :filter => "UoW.Specs*")
end

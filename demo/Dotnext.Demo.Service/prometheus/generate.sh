./gomplate_windows-amd64-slim.exe \
	-f demo_service_rules.yml \
	-c .=config.production.yml \
	-o demo_service_rules.production.yml \
	--left-delim '${' \
	--right-delim '}$'
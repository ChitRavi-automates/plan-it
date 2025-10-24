# This script interacts with the Portainer API to perform different requests
# Used in managing docker instances
import sys
import json
import re
import httpx
import os
import argparse
import pprint as pp

# Setup for variables we need to access
mode: str = ""
response: httpx.Response
request_data: dict = {}
request_files: dict = {}
request_header: dict = {}
request_parameters: str = ""
request_json: str = ""
request_endpoint: str = ""

# Constants from GitLab variables
ENDPOINT_NAME = ""
PORTAINER_PW = ""
PORTAINER_USER = ""
PORTAINER_URL = ""
STACK_NAME = ""
CI_REGISTRY = ""
CI_PROJECT_NAMESPACE = ""
CI_PROJECT_NAME = ""
CI_COMMIT_REF_NAME = ""
CI_DEFAULT_BRANCH = ""
CI_API_TOKEN = ""
CI_COMMIT_REF_SLUG = ""
CI_MERGE_REQUEST_PROJECT_ID = ""
CI_MERGE_REQUEST_IID = ""

# Get flags for what we are doing
parser = argparse.ArgumentParser()

# Define flags that can be set
parser.add_argument("mode", default="", help="[d]eploy or [t]eardown")
parser.add_argument(
    "-dc", "--dockercompose", default="", help="Path to docker-compose file"
)
parser.add_argument(
    "--debug", action="store_true", help="Flag for debug run, dont deploy/teardown"
)

args = parser.parse_args()

# Store variables
mode = args.mode
DOCKER_COMPOSE_FILE = args.dockercompose
debug = args.debug


# Get environment variables
def getEnvironmentVars(vars: list):
    errors = []
    for var in vars:
        try:
            globals()[var] = os.environ[var]
        except KeyError as error:
            errors.append(
                f"ERROR: Value for key '{error.args[0]}' not present in environment"
            )
    # If errors isnt empty, exit program as variables are missing
    if len(errors) > 0:
        for error in errors:
            print(error)
        sys.exit(1)


# Sets the request_endpoint variable to what is given
def setAPIEndpoint(endpoint: str):
    global request_endpoint
    request_endpoint = f"{PORTAINER_URL}/{endpoint}"


# Adds a query to end of API call with given string TODO: convert to work with multiple inputs
def setRequestParameters(params: str):
    global request_endpoint
    request_endpoint = request_endpoint + "?" + params


# Checks response and prints request data if wrong code recieved
def checkResponse(request: str):
    # If code is 200 or 204 and DELETE request, it was a success
    if response.is_success:
        # If no error, set all requests to empty string again
        resetRequests()
        return
    print(
        f"ERROR: Response code received is {response.status_code} from {request}-request"
    )
    print(f"ENDPOINT: {request_endpoint}")
    # Utilize regex to substitute the password, so it doesnt show in logs
    if request_data:
        print(
            f"DATA: {re.sub('password\': .*\'', f"PW_LEN: {len(PORTAINER_PW)}\'", request_data.__str__())}"
        )
    if request_files:
        print(f"FILES: {request_files}")
    if request_json:
        print(f"JSON: {request_json}")
    print(f"TEXT: {response.text}")
    sys.exit(1)


# Sets requests to empty string
def resetRequests():
    global request_endpoint, request_data, request_json, request_files
    request_endpoint = ""
    request_json = ""
    request_data = {}
    request_files = {}


# Gets token from API
def getToken():
    global response, request_endpoint, request_data, request_header
    getEnvironmentVars(["PORTAINER_USER", "PORTAINER_PW"])

    # Get token
    print("\nMaking request for token to API")
    setAPIEndpoint("auth")
    request_data = {"username": PORTAINER_USER, "password": PORTAINER_PW}
    response = httpx.post(url=request_endpoint, json=request_data, verify=False)
    checkResponse("POST")

    token = json.loads(response.text)["jwt"]
    request_header = {"Authorization": f"Bearer {token}"}
    print(f"Token received of length: {len(token)}")


# Gets endpointID based on ENDPOINT_NAME
def getEndpointData() -> tuple:
    global response, request_endpoint, request_data, request_header
    getEnvironmentVars(["ENDPOINT_NAME"])

    # Get which endpoint ID we are deploying to
    print(f"\nGetting ENDPOINT_ID and SWARM_ID for endpoint with name: {ENDPOINT_NAME}")
    setAPIEndpoint("endpoints")
    setRequestParameters(ENDPOINT_NAME)
    response = httpx.get(url=request_endpoint, headers=request_header, verify=False)
    checkResponse("GET")
    endpoint_data = json.loads(response.text)

    # Extract endpoint ID
    endpoint_id = endpoint_data[0]["Id"]
    print(f"ENDPOINT_ID received: {endpoint_id}")

    # Extract swarm ID
    swarm_id = json.loads(response.text)[0]["Snapshots"][0]["DockerSnapshotRaw"][
        "Info"
    ]["Swarm"]["Cluster"]["ID"]
    print(f"SWARM_ID received: {swarm_id}\n")
    return endpoint_id, swarm_id


# Checks if STACK_NAME is already deployed, returns empty string if STACK_NAME doesnt exist
# Returns STACK_ID if exists, to update
def isStackDeployed() -> str:
    global response, request_endpoint, request_data, request_header
    stack_id = ""

    # Setup
    setAPIEndpoint("stacks")
    response = httpx.get(url=request_endpoint, headers=request_header, verify=False)
    checkResponse("GET")
    stack_data = json.loads(response.text)

    # Check if STACK_NAME exists
    for stack in stack_data:
        if stack["Name"] == STACK_NAME:
            print(f"Stack with name {STACK_NAME} exists with ID: {stack['Id']}")
            return stack["Id"]
    print(f"Stack with name {STACK_NAME} doesn't exist")
    return stack_id

# Posts a comment with links to db and api on a merge request when a new stack is deployed
def postMergeRequestCommentWithUrls():
    global response, request_endpoint, request_header, request_json

    # If on main, dont post a comment
    if CI_COMMIT_REF_NAME == CI_DEFAULT_BRANCH:
        return

    # Setup
    getEnvironmentVars([
        "CI_API_TOKEN",
        "CI_COMMIT_REF_SLUG",
        "CI_MERGE_REQUEST_PROJECT_ID",
        "CI_MERGE_REQUEST_IID",
    ])

    # Generate comment for the MR
    baseURL = f"g13-{CI_COMMIT_REF_SLUG}.cc25.chasacademy.dev"
    commentContent = f"[MAIN](https://{baseURL}) - [API URL](https://api.{baseURL}) - [DB URL](https://db.{baseURL})"

    # Setup quest
    request_endpoint = (f"https://git.chasacademy.dev/api/v4/projects/{CI_MERGE_REQUEST_PROJECT_ID}/merge_requests/{CI_MERGE_REQUEST_IID}/notes")
    request_header = {"PRIVATE-TOKEN": CI_API_TOKEN, "Content-Type": "application/json"}

    data = {"body": commentContent}
    request_json = json.dumps(data)  # Dump into a JSON-string
    request_json = json.loads(request_json)  # Create JSON-object from JSON-string

    # Post comment
    response = httpx.post(
        url=request_endpoint,
        headers=request_header,
        json=request_json,
        verify=False,
    )
    checkResponse("POST")

# Parses docker-compose file and fills in environment vars
def parseDockerCompose() -> str:
    try:
        f = open(DOCKER_COMPOSE_FILE, "r")
    except OSError:
        print("ERROR: docker-compose file could not be opened")
        sys.exit(1)

    expanded = os.path.expandvars(f.read())
    f.close()
    return expanded

# Gets correct endpoint for where to deploy swarm
def deploy():
    global response, request_endpoint, request_data, request_header, request_files, request_json

    # Check if pipeline is running on branch
    if CI_COMMIT_REF_NAME == CI_DEFAULT_BRANCH:
        # On main branch
        print(f"On branch: {CI_DEFAULT_BRANCH}, dockertag: latest")
        os.environ["DOCKER_TAG"] = "latest"
    else:
        print(f"On branch: {CI_COMMIT_REF_NAME}, dockertag: {CI_COMMIT_REF_NAME}")
        os.environ["DOCKER_TAG"] = CI_COMMIT_REF_NAME

    swarm_config = parseDockerCompose()

    # Check if STACK_NAME is present
    stack_id = isStackDeployed()

    if stack_id:  # Not empty string, stack exists
        # Set up API call to update stack
        setAPIEndpoint(f"stacks/{stack_id}")
        setRequestParameters(f"endpointId={endpoint_id}")

        # Format JSON payload
        modified_swarm_config = {
            "prune": True,
            "pullImage": True,
            "stackFileContent": swarm_config,
        }
        request_json = json.dumps(modified_swarm_config)  # Dump into a JSON-string
        request_json = json.loads(request_json)  # Create JSON-object from JSON-string

        if debug:
            print("Ending program, debug run only")
            sys.exit(0)

        # Update stack
        response = httpx.put(
            url=request_endpoint,
            headers=request_header,
            json=request_json,
            verify=False,
        )
        checkResponse("PUT")
        print("Update of stack successful")
    else:
        # Set up API call to deploy new stack
        setAPIEndpoint("stacks/create/swarm/file")
        setRequestParameters(f"endpointId={endpoint_id}")
        request_files = {"file": ("swarm-config", swarm_config)}
        request_data = {"Name": STACK_NAME, "SwarmID": swarm_id}

        if debug:
            print("Ending program, debug run only")
            sys.exit(0)

        # Deploy new stack
        response = httpx.post(
            url=request_endpoint,
            headers=request_header,
            data=request_data,
            files=request_files,
            verify=False,
        )
        checkResponse("POST")
        print("Deployment of stack successful")

        # Comment
        print("Posting comment on MR")
        postMergeRequestCommentWithUrls()

    # Print information recevied after successful request
    print("-- Stack information --")
    pp.pprint(response.json())


def teardown():
    global response, request_endpoint, request_data, request_header, request_files, request_json

    # Check if STACK_NAME is present
    stack_id = isStackDeployed()

    if stack_id:  # Not empty string, is deployed
        # Set up API call to remove stack
        setAPIEndpoint(f"stacks/{stack_id}")
        setRequestParameters(f"endpointId={endpoint_id}")

        if debug:
            print("Ending program, debug run only")
            sys.exit(0)

        # Remove stack
        response = httpx.delete(
            url=request_endpoint, headers=request_header, verify=False
        )
        checkResponse("DELETE")
        print(f"Deletion of stack {STACK_NAME} successful")
    else:
        sys.exit(1)


# --- Start procedures here ---
# Base URL to API and image registry
getEnvironmentVars(["PORTAINER_URL", "CI_REGISTRY"])

# Get token for Portainer API
getToken()

# Get endpoint id and swarm id
endpoint_id, swarm_id = getEndpointData()

# Get all relevant pipeline variables from OS environment for deploy/teardown jobs
getEnvironmentVars(
    [
        "CI_PROJECT_NAMESPACE",
        "CI_PROJECT_NAME",
        "CI_COMMIT_REF_NAME",
        "CI_DEFAULT_BRANCH",
        "STACK_NAME",
    ]
)

if mode[0] == "d":
    mode = "deploy"
    print(f"Mode specified: {mode}")
    deploy()
elif mode[0] == "t":
    mode = "teardown"
    print(f"Mode specified: {mode}")
    teardown()
else:
    print("ERROR: No mode specified.")
    sys.exit(1)

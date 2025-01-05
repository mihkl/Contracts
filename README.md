# THIS PROJECT WAS ORIGINALLY DEVELOPED IN GITLAB SO ALL CI/CD PIPELINES WILL NOT WORK ON GITHUB

# Intended for contract management and signing by simplifying and automating some of the steps like generating from templates and electronic signature verification.

# ITB2203_2024_Lepingud

## Running the backend locally

1. Make sure that Docker has been installed on the machine where the app should run locally (https://www.docker.com/products/docker-desktop/).

2. Open up a terminal. Change directory into the /Backend/API folder, where the Dockerfile is located. `cd Backend/API`.

3. Once in the API directory, run `docker build -t "isa3" . `. "isa3" is just a name for the image, and can be replaced with any other name. In case of `Docker daemon not running` error, just open up the docker desktop app and retry the command. The build process can take 10+ minutes the first time, when layers have not been cached.

4. After the build has completed, run `docker images` and verify that the image has been created with the name selected in the previous step.

5. If the image has been successfully created, run `docker run -p 5143:8080 -e HMACSecretKey=123456 -e ASPNETCORE_ENVIRONMENT=Development isa3`, where isa3 is the name of the image selected in step 3 and HMACSecretKey is equal to secret key used when generating HMAC signatures (in the example above, the secret key is going to have a value of 123456).

6. Verify that the api is running by navigating to `http://localhost:8080/api/contracts` in the browser.

<img width="469" alt="upload" src="https://github.com/user-attachments/assets/aba7ae80-f659-47e3-8a5b-0bce2f49dba2" />
<img width="679" alt="generate" src="https://github.com/user-attachments/assets/81b15bc8-e38c-49a9-bd7d-b3af897fc1f7" />
<img width="833" alt="submissions" src="https://github.com/user-attachments/assets/7e2e4174-1470-4503-aadc-52713b3f0083" />

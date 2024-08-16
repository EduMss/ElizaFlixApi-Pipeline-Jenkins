pipeline {
    agent any
    stages {
        stage('Checkout'){
            steps {
                checkout([$class: 'GitSCM', branches: [[name: '*/master']], userRemoteConfigs: [[url: 'https://github.com/EduMss/ElizaFlixApi-Pipeline-Jenkins.git']]])
            }
        }

        // stage('Build-Windows') {
        //     steps {
        //         bat 'dotnet build --configuration Release'
        //     }
        // }

        // stage('Teste') {
        //     steps {
        //         bat 'dotnet test'
        //     }
        // }

        // stage('Clear Debug Project'){
        //     steps {
        //         bat 'dotnet clean --configuration Debug'
        //         // bat 'rd /s /q bin\\Debug'
        //     }
        // }


        //Variaveis de ambiente Jenkins:
        // env.JOB_NAME: Nome do job Jenkins.
        // env.BUILD_ID: Um ID exclusivo para o build atual.
        // env.BUILD_TAG: Uma tag legível que combina o nome do job e o número do build.
        // env.WORKSPACE: Diretório de trabalho atual do job.
        // env.BUILD_NUMBER: Retorna o numero da Buildo do Jenkins

        // stage('Docker Login') {
        //     //Plugins Instalados:
        //     //Docker Pipeline Plugin
        //     //Docker Commons Plugin

        //     //DOCKER_CREDENTIALS_ID -> fui uma credencial criada no jenkins com as credenciais do docker hub.
        //     steps {
        //         script {
        //             withCredentials([usernamePassword(credentialsId: 'DOCKER_CREDENTIALS_ID', passwordVariable: 'password', usernameVariable: 'username')]){
        //                 bat '''
        //                     echo "${password} | docker login -u ${username} --password-stdin"
        //                 '''
        //                 def app = docker.build("edumss/elizaflixapi")
        //                 app.push("latest")
        //                 app.push("${env.BUILD_NUMBER}")
        //             }
        //         }
        //     }
        // }


        stage('SonarQube Scan') {
            steps {
                withSonarQubeEnv(installationName: 'SonarQubeServer') {
                    
                }
            }
        }
    }
}
pipeline {
    agent any
    stages {
        stage('Checkout'){
            steps {
                checkout([$class: 'GitSCM', branches: [[name: '*/master']], userRemoteConfigs: [[url: 'https://github.com/EduMss/ElizaFlixApi-Pipeline-Jenkins.git']]])
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build'
            }
        }

        stage('Teste') {
            steps {
                bat 'dotnet teste ElizaFlixAPI.csproj'
            }
        }
    }
}
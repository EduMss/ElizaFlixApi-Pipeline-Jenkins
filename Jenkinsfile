pipeline {
    agent any
    stages {
        stage('Checkout'){
            steps {
                checkout([$class: 'GitSCM', branches: [[name: '*/master']], userRemoteConfigs: [[url: 'https://github.com/EduMss/ElizaFlixApi-Pipeline-Jenkins.git']]])
            }
        }

        // stage('Build') {
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

        stage('Docker Login') {
            steps {
                script {
                    // docker.withRegistry('', DOCKER_CREDENTIALS_ID) {
                    //     // Esse bloco realiza o login automaticamente
                    //     // e executa qualquer comando Docker dentro desse bloco com o login ativo.
                    //     // Você pode incluir outros passos aqui, como build, push, etc.
                    //     bat 'docker build -t edumss/elizaflixapi:latest .'
                    //     bat 'docker push edumss/elizaflixapi:latest'
                    // }

                    withCredentials([usernamePassword(credentialsId: 'DOCKER_CREDENTIALS_ID', passwordVariable: 'password', usernameVariable: 'username')]){
                        bat '''
                            echo "${password} | docker login -u ${username} --password-stdin"
                        '''
                        def app = docker.build("edumss/elizaflixapi")
                        app.push("latest")
                        app.push("${env.BUILD_NUMBER}")
                    }
                }
            }
        }
        // stage('Build Image') {
        //     steps {
        //         script {
        //             // Comando para buildar a imagem
        //             // sh 'docker build -t edumss/elizaflixapi .'
        //             bat 'docker build -t edumss/elizaflixapi:latest .'// -t edumss/elizaflixapi: .'
        //         }
        //     }
        // }
        // stage('Push Image') {
        //     steps {
        //         script {
        //             // Comando para fazer o push da imagem para o Docker Registry
        //             bat 'docker push edumss/elizaflixapi:latest'
        //         }
        //     }
        // }
    }
}
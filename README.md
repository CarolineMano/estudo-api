<h1 align="center">Estudo API</h1> 

<p style="text-align: justify">Implementação de uma plataforma simples de MOOC (Massive Open Online Course), onde estudantes podem se matricular em cursos e instrutores podem adicionar, editar e deletar cursos. O sistema é uma releitura do programa realizado no Estudo MVC, aplicando os conceitos de uma API RESTful.</p>

---
## :memo: Descrição do Programa

**<h3>Entidades do sistema</h3>**
<p style="text-align: justify">Com a intenção de ser um programa didático, o sistema possui três entidades básicas:</p>

- User: usuário do sistema, que pode ser tanto um estudante quanto um instrutor, extende da classe IdentityUser e implementa novas propriedades de nome e sobrenome.

- Course: entidade responsável por armazenar os dados básicos de um curso cadastrado, inclusive o Id e nome do instrutor responsável.

- Enrollment: relacionamento entre um estudante e um curso em que está cadastrado. 

**<h3>Controladores</h3>**
<p style="text-align: justify">Existem três controladores distintos no programa: </p>

- AccountController: realiza o registro e login de usuários a partir das tabelas existentes no Identity. Quando do login, irá gerar um token JWT, necessário para que os usuários possam acessar diferentes funcionalidades do programa, a depender do seu perfil (estudante/instrutor).

- CourseController: responsável por listar os cursos disponíveis, independente do perfil do usuário, além de permitir aos instrutores a adição, deleção e edição de cursos. Um instrutor poderá editar e deletar os cursos que foram adicionados por ele, mas não por outros instrutores. Além disso, um curso só poderá ser excluído se não houver estudantes matriculados.

- EnrollmentController: gerencia a matrícula e desmatrícula de estudantes dos cursos cadastrados. Um estudante não poderá se inscrever no mesmo curso duas vezes e poderá ver apenas as suas matrículas, mas não as de outros alunos. Os instrutores não têm acesso a essa funcionalidade.

---
## :floppy_disk: Banco de Dados

<p style="text-align: justify">MySql foi configurado na porta 3306, com uid e senha iguais a root. O banco de nome estudo_api_mano será criado automaticamente quando a aplicação for iniciada pela primeira vez.
Será criado automaticamente um registro de cada uma das entidades acima relacionadas, além de dois registros na tabela Roles e os respectivos registros na tabela UserRoles.</p>

<p style="text-align: justify">Os dados de login dos usuários criados automaticamente são os seguintes: </p>

<p style="text-align: justify"></p>

| Perfil | Usuário | Senha |
|------|---------|------|
| Faculty | joel@email.com | MyPass_w0rd |
| Student | jacques@email.com | MyPass_w0rd |

---
## :notebook_with_decorative_cover:	Documentação

<p style="text-align: justify">Swagger foi utilizado para a documentação da API. Após a inicialização da aplicação, haverá uma breve descrição de seus endpoints, além das entradas esperadas e as possíveis saídas. Através do Swagger, será possível também testar a API, tendo-se em mente que para algumas funcionalidades será necessário gerar um token e autorizá-lo através do botão "Authorize" no canto superior direito da tela. </p>

---

---

<h1 align="center">API Study</h1>
<p style="text-align: justify">Implementation of a simple MOOC (Massive Open Online Course) plataform, on which students can enroll in courses and instructors can add, edit and delete courses. The system is a new version of the program written for MVC Study, now using the concepts of a RESTful API.</p>

---
## :memo: Program Description

**<h3>System Entities</h3>**
<p style="text-align: justify">Intended to be a didactic program, the system has three basic entities:</p>

- User: it's the system user, which can be either a student or an instructor, this class extends from the IdentityUser class and implements new properties (given name, surname and full name).

- Course: entity responsible for storing the basic data of a registered course, which includes the Id and the name of the responsible instructor.

- Enrollment: relationship between the student and a course they are enrolled in. 

**<h3>Controllers</h3>**
<p style="text-align: justify">There are three distinct controllers in the program: </p>

- AccountController: performs user registration and login based on the existing Identity tables. After a successfull login, it will generate a JWT token, which is necessary for user authentication and access authorization to some features. The authorization might depend on the role (student/instructor). 

- CourseController: responsible for listing available courses, regardless of user profile, in addition to allowing instructors to add, delete and edit courses. An instructor will not be able to edit and delete courses that were added by another instructor. Furthermore, a course can only be excluded if there are no students enrolled in. 

- EnrollmentController: manages the enrollment and unenrollment of students. A student will not be able to enroll in the same course twice and can only see their own enrollment list, but not that of other students. Instructor do not have access to this functionality.

---

## :floppy_disk: Database

<p style="text-align: justify">MySql was configured on port 3306, with uid and password equal to root. The database named estudo_api_mano will be created automatically when the application is first launched. One record for each of the entities listed above will be automatically created, in addition to two records in the Roles table and the necessary registries for UserRoles.</p>

<p style="text-align: justify">The automatically created user log data are as follows: </p>

| Role | User | Password |
|------|---------|------|
| Faculty | joel@email.com | MyPass_w0rd |
| Student | jacques@email.com | MyPass_w0rd |

---
## :notebook_with_decorative_cover:	Documentation

<p style="text-align: justify">It was used Swagger for API documentation. As soon as the application is launched, there will be a brief description of its endpoints, as well as the expected inputs and possible outputs. It's also possible to test the API via Swagger, bearing in mind that some features require a valid JWT token to be generated via login and validaded with the "Authorize" button found in the upper right corner of the screen.</p>